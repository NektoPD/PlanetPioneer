using System;
using TMPro;
using UnityEngine;

public class PlayerResourcesView : MonoBehaviour
{
    [SerializeField] private TMP_Text _ironAmount;

    private CatchedResourceHandler _resourceHandler;

    private void SetAmount(int amount)
    {
        _ironAmount.text = amount.ToString();
    }

    public void SetResourceHandler(CatchedResourceHandler resourceHandler)
    {
        if (resourceHandler == null)
            throw new ArgumentNullException();

        _resourceHandler = resourceHandler;
        _resourceHandler.IronAmountChanged += SetAmount;
    }
}
