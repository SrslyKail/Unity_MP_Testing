
public abstract class Coin : Pickup<int>
{
    protected int CoinValue;

    public void SetValue(int value)
    {
        CoinValue = value;
    }
}
