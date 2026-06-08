
using System;
using UnityEngine;

public class RespawningCoin : Coin
{
    public override event Action<Pickup<int>> OnCollected;
    private Vector3 _prevPosition;

    private void Start()
    {
        _prevPosition = transform.position;
    }

    private void Update()
    {
        if (IsServer)
        {
            return;
        }

        // if (!_prevPosition.Equals(transform.position))
        // {
        //     Show(true);
        //     _prevPosition = transform.position;
        // }
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
