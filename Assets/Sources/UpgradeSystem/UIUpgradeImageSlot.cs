using System;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeImageSlot : MonoBehaviour
{
    private Image _image;
    private bool _isActivated = false;

    public bool Activated => _isActivated;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SetUpgradedImage(Sprite upgradedImage)
    {
        if(upgradedImage == null)
            throw new ArgumentNullException(nameof(upgradedImage));

        _image.sprite = upgradedImage;
        _isActivated = true;
    }
}