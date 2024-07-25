using System;
using UnityEngine;

public class WeaponUpgrader : MonoBehaviour
{
    private const string WeaponSecondUpgradeMessage = "Weapon upgraded, you can now collect Crystals";
    private const string WeaponThirdUpgradeMessage = "Weapon upgraded, you can now collect Plant";
    private const string WeaponLastUpgradeMessage = "Weapon upgraded, you can now collect Alien Artifact";
    private const string WeaponFullyUpgradedMessage = "Weapon is fully upgraded";

    [SerializeField] private UIPopUpWindowShower _windowShower;

    private const int WeaponStartLevel = 1;
    private const int WeaponSecondLevel = 2;
    private const int WeaponThirdLevel = 3;
    private const int WeaponEndLevel = 4;

    private int _currentLevel = 1;

    public event Action WeaponUpgraded;
    public event Action WeaponFullyUpgraded;

    public int StartLevel => WeaponStartLevel;
    public int SecondLevel => WeaponSecondLevel;
    public int ThirdLevel => WeaponThirdLevel;
    public int EndLevel => WeaponEndLevel;
    public int CurrentLevel => _currentLevel;

    private void Start()
    {
        _currentLevel = WeaponStartLevel;
    }

    public void UpgradeWeapon()
    {
        if (_currentLevel < WeaponEndLevel)
        {
            _currentLevel++;
            WeaponUpgraded?.Invoke();

            switch (_currentLevel)
            {
                case WeaponSecondLevel:
                    _windowShower.AddMessageToQueue(WeaponSecondUpgradeMessage);
                    break;
                case WeaponThirdLevel:
                    _windowShower.AddMessageToQueue(WeaponThirdUpgradeMessage);
                    break;
                case WeaponEndLevel:
                    _windowShower.AddMessageToQueue(WeaponLastUpgradeMessage);
                    break;
            }

            if (_currentLevel >= WeaponEndLevel)
            {
                WeaponFullyUpgraded?.Invoke();
                _windowShower.AddMessageToQueue(WeaponFullyUpgradedMessage);
            }
        }
    }
}
