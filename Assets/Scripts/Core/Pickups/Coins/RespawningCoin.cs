
using System;
using UnityEngine;

public class RespawningCoin : Coin
{
    public override event Action<Pickup<int>> OnCollected;

    private void Update()
    {
        if (IsServer)
        {
            return;
        }
        if (transform.hasChanged)
        {
            Show(true);
        }
    }

    public override int Collect()
    {
        if (!IsServer)
        {
            Show(false);
            return 0;
        }

        if (IsCollected)
        {
            return 0;
        }
   
        IsCollected = true;

        OnCollected?.Invoke(this);
        return coinValue;
    }
    
}
