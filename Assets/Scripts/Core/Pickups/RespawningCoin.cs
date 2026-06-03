
public class RespawningCoin : Coin
{
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
        return CoinValue;
    }
}
