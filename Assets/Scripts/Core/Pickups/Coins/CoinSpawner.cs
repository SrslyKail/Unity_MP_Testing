using Unity.Netcode;
using UnityEngine;

public class CoinSpawner : NetworkBehaviour
{
    [SerializeField] private RespawningCoin coinPrefab;

    [SerializeField] private int maxCoins = 50;
    [SerializeField] private int coinValue = 10;

    [SerializeField] private Vector2 xSpawnRange;
    [SerializeField] private Vector2 ySpawnRange;
    [SerializeField] private LayerMask layerMask;

    private float _coinRadius;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }

        _coinRadius = coinPrefab.GetComponent<CircleCollider2D>().radius;
        for (int _ = 0; _ < maxCoins;  ++_)
        {
            SpawnCoin();
        }
    }

    private void SpawnCoin()
    {
        RespawningCoin coinInstance = Instantiate(
            coinPrefab,
            GetSpawnPoint(),
            Quaternion.identity
        );
        coinInstance.SetValue(coinValue);
        coinInstance.GetComponent<NetworkObject>().Spawn();

        coinInstance.OnCollected += HandleCoinCollected;
    }

    private void HandleCoinCollected(Pickup<int> coin)
    {
        coin.transform.position = GetSpawnPoint();
        coin.Reset();
    }

    private Vector2 GetSpawnPoint()
    {
        while (true)
        {
            Vector2 spawnPoint = new Vector2(
                Random.Range(xSpawnRange.x, xSpawnRange.y),
                Random.Range(ySpawnRange.x, ySpawnRange.y)
            );
            Collider2D hitCollider =
                Physics2D.OverlapCircle(spawnPoint, _coinRadius, layerMask);
            if (hitCollider == null)
            {
                return spawnPoint;
            }
        }
    }
}