using System;
using UnityEngine;
using Zenject;

public class PlayerGoldHandler : MonoBehaviour,IGoldHandler
{
    private const string MaximumGoldAmounReachedErrorMessage = "Maximum gold amount reached";
    private const string NotEnoughGoldToDecreaseErrorrMessage = "Not enough gold to decrease by the specified amount";

    private UIPopUpWindowShower _windowShower;

    private int _goldMultiplier = 10;
    private int _goldAmount = 0;
    private int _goldMaxAmount = 9999;

    public event Action GoldReceived;
    public event Action<int> GoldAmountChanged;
    
    public int GoldAmount => _goldAmount;

    [Inject]
    private void Construct(UIServicesProvider UIServices)
    {
        _windowShower = UIServices.PopUpWindow;
    }

    public void IncreaseGoldAmount()
    {
        if (_goldAmount >= _goldMaxAmount)
        {
            _windowShower.AddMessageToQueue(MaximumGoldAmounReachedErrorMessage);
            return;
        }

        _goldAmount += _goldMultiplier;
        GoldReceived?.Invoke();
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

    public void SetGoldAmount(int amount)
    {
        if (amount < 0 || amount > _goldMaxAmount)
            throw new ArgumentOutOfRangeException(nameof(amount));

        _goldAmount = amount;
    }
}
