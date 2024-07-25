using System;
using TMPro;
using UnityEngine;

public class PlayerGoldView : MonoBehaviour
{
    [SerializeField] private TMP_Text _goldAmount;

    private PlayerGoldHandler _handler;

    private void SetAmount(int amount)
    {
        _goldAmount.text = amount.ToString();
    }

    public void SetGoldHandler(PlayerGoldHandler goldHandler)
    {
        if (goldHandler == null)
            throw new ArgumentNullException();

        _handler = goldHandler;
        _handler.GoldAmountChanged += SetAmount;
    }
}
