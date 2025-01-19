using System;
using TMPro;
using UnityEngine;

public class PlayerGoldView : MonoBehaviour
{
    [SerializeField] private TMP_Text _goldAmount;
    [SerializeField] private PlayerGoldHandler _playerGoldHandler;

    private IGoldHandler _handler;

    private void Start()
    {
        SetAmount(_playerGoldHandler.GoldAmount);
    }

    private void SetAmount(int amount)
    {
        _goldAmount.text = amount.ToString();
    }

    public void SetGoldHandler(IGoldHandler goldHandler)
    {
        if (goldHandler == null)
            throw new ArgumentNullException();
        
        _handler = goldHandler;
        _handler.GoldAmountChanged += SetAmount;
    }
}
