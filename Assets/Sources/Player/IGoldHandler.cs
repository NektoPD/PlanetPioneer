using System;

public interface IGoldHandler
{
    public event Action AmountChanged;
    public event Action<int> GoldAmountChanged;
    public int GoldAmount { get; }
    public void SetGoldAmount(int amount);
}