
using UnityEngine;

public abstract class Coin : Pickup<int>
{
    [SerializeField] protected int coinValue;

    public void SetValue(int value)
    {
        coinValue = value;
    }

    public override void Reset()
    {
        IsCollected = false;
    }
}
