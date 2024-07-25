using System;
using UnityEngine;

public class PlayerGoldHandler : MonoBehaviour
{
    private const string MaximumGoldAmounReachedErrorMessage = "Maximum gold amount reached";
    private const string NotEnoughGoldToDecreaseErrorrMessage = "Not enough gold to decrease by the specified amount";

    [SerializeField] private UIPopUpWindowShower _windowShower;

    private int _goldMultiplier = 100;
    private int _goldAmount = 0;
    private int _goldMaxAmount = 9999;

    public int GoldAmount => _goldAmount;

    public event Action<int> GoldAmountChanged;

    public void SetGoldAmount()
    {
        if (_goldAmount >= _goldMaxAmount)
        {
            _windowShower.AddMessageToQueue(MaximumGoldAmounReachedErrorMessage);
            return;
        }

        _goldAmount += _goldMultiplier;
        GoldAmountChanged?.Invoke(_goldAmount);
    }

    public void DecreaceGoldAmount(int amount)
    {
        if(amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        if (_goldAmount - amount < 0)
        {
            _windowShower.AddMessageToQueue(NotEnoughGoldToDecreaseErrorrMessage);
            return;
        }

        _goldAmount -= amount;
        GoldAmountChanged?.Invoke(_goldAmount);
    }
}
