
using UnityEngine;

public abstract class Coin : Pickup<int>
{
    [SerializeField] protected int CoinValue;

    public void SetValue(int value)
    {
        CoinValue = value;
    }
}
