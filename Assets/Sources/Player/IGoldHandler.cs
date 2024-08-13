using System;

public interface IGoldHandler
{
    public event Action GoldReceived;
    public event Action<int> GoldAmountChanged;
}