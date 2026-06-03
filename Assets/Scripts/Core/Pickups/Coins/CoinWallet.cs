using System;
using Unity.Netcode;
using UnityEngine;

public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> TotalCoins = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Attempting to pickup");
        if (!other.TryGetComponent(out Coin coin))
        {
            return;
        }

        int coinValue = coin.Collect();
        if (!IsServer)
        {
            return;
        }
        
        TotalCoins.Value += coinValue;
        Debug.Log($"Picked up {coinValue} coins");
        Debug.Log($"Currently have {TotalCoins.Value} coins");
    }
}
