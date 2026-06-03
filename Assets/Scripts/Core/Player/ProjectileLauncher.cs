using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class ProjectileLauncher : NetworkBehaviour
{
    [Header("References")] 
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject serverProjectilePrefab;
    [SerializeField] private GameObject clientProjectilePrefab;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private Collider2D playerCollider;
    [Header("Settings")] 
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float fireRatePerSecond;
    [SerializeField] private float muzzleFlashDurationSeconds;

    private bool _shouldFire = false;
    private float _previousFireTime;
    private float _muzzleFlashTimer;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }

        inputReader.PrimaryFireEvent += HandlePrimaryFire;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }

        inputReader.PrimaryFireEvent -= HandlePrimaryFire;
    }

    private void Update()
    {
        if (_muzzleFlashTimer > 0f)
        {
            _muzzleFlashTimer -= Time.deltaTime;
            if (_muzzleFlashTimer <= 0f)
            {
                muzzleFlash.SetActive(false);
            }
        }
        
        if (!IsOwner)
        {
            return;
        }

        if (!_shouldFire)
        {
            return;
        }

        //Check if we're allowed to fire.
        if (Time.time < (1 / fireRatePerSecond) + _previousFireTime)
        {
            return;
        }
        
        PrimaryFireServerRpc(projectileSpawnPoint.position, projectileSpawnPoint.up);
        SpawnDummyProjectile(projectileSpawnPoint.position, projectileSpawnPoint.up);
        
        _previousFireTime = Time.time;
    }

    private void HandlePrimaryFire(bool requestFire)
    {
        this._shouldFire = requestFire;
    }

    [ServerRpc]
    private void PrimaryFireServerRpc(Vector3 spawnPosition, Vector3 spawnDirection)
    {
        GameObject projectile = SpawnProjectile(serverProjectilePrefab, spawnPosition, spawnDirection);
        if (projectile.TryGetComponent<DealDamageOnContact>(out DealDamageOnContact damageOnContact))
        {
            damageOnContact.SetOwner(this.OwnerClientId);
        }
        PrimaryFireClientRpc(spawnPosition, spawnDirection);
    }
    
    [ClientRpc]
    private void PrimaryFireClientRpc(Vector3 spawnPosition, Vector3 spawnDirection)
    {
        if (!IsOwner)
        {
            SpawnDummyProjectile(spawnPosition, spawnDirection);
        }
    }

    private void SpawnDummyProjectile(Vector3 spawnPosition, Vector3 spawnDirection)
    {
        muzzleFlash.SetActive(true);
        _muzzleFlashTimer = muzzleFlashDurationSeconds;
        
        SpawnProjectile(clientProjectilePrefab, spawnPosition, spawnDirection);

    }

    private GameObject SpawnProjectile(GameObject projectilePrefab, Vector3 spawnPosition, Vector3 spawnDirection)
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        projectile.transform.up = spawnDirection;
        
        Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), playerCollider);

        if (projectile.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.linearVelocity = rb.transform.up * projectileSpeed;
        }
        
        return projectile;
    }

    private IEnumerator ActivateMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(_muzzleFlashTimer);
        muzzleFlash.SetActive(false);
    }
}