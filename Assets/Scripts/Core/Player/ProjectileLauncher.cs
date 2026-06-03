using Unity.Netcode;
using UnityEngine;

public class ProjectileLauncher : NetworkBehaviour
{
    [Header("References")] [SerializeField]
    private InputReader inputReader;

    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject serverProjectilePrefab;
    [SerializeField] private GameObject clientProjectilePrefab;
    [Header("Settings")] [SerializeField] private float projectileSpeed;

    private bool shouldFire = false;

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
        if (!IsOwner)
        {
            return;
        }

        if (!shouldFire)
        {
            return;
        }
        
        SpawnProjectileServerRpc(projectileSpawnPoint.position, projectileSpawnPoint.up);
        SpawnDummyProjectile(projectileSpawnPoint.position, projectileSpawnPoint.up);
    }

    private void HandlePrimaryFire(bool requestFire)
    {
        this.shouldFire = requestFire;
    }

    [ServerRpc]
    private void SpawnProjectileServerRpc(Vector3 spawnPosition, Vector3 spawnDirection)
    {
        GameObject projectile = Instantiate(serverProjectilePrefab, spawnPosition, Quaternion.identity);
        projectile.transform.up = spawnDirection;
        SpawnProjectileClientRpc(spawnPosition, spawnDirection);
    }
    
    [ClientRpc]
    private void SpawnProjectileClientRpc(Vector3 spawnPosition, Vector3 spawnDirection)
    {
        if (!IsOwner)
        {
            SpawnDummyProjectile(spawnPosition, spawnDirection);
        }
    }

    private void SpawnDummyProjectile(Vector3 spawnPosition, Vector3 spawnDirection)
    {
        GameObject projectile = Instantiate(clientProjectilePrefab, spawnPosition, Quaternion.identity);
        projectile.transform.up = spawnDirection;
    }
}