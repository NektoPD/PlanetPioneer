using System;
using UnityEngine;
using Zenject;

public class WeaponUpgrader : MonoBehaviour,IWeaponUpgrader
{
    private const string WeaponSecondUpgradeMessage = "Weapon upgraded, you can now collect Crystals";
    private const string WeaponThirdUpgradeMessage = "Weapon upgraded, you can now collect Plant";
    private const string WeaponLastUpgradeMessage = "Weapon upgraded, you can now collect Alien Artifact";
    private const string WeaponFullyUpgradedMessage = "Weapon is fully upgraded";

    private const int WeaponStartLevel = 1;
    private const int WeaponSecondLevel = 2;
    private const int WeaponThirdLevel = 3;
    private const int WeaponEndLevel = 4;

    private UIPopUpWindowShower _windowShower;
    private int _currentLevel = 1;

    public event Action WeaponUpgraded;
    public event Action WeaponFullyUpgraded;
    public event Action WeaponSecondLevelUpgraded;
    public event Action WeaponThirdLevelUpgraded;

    public int StartLevel => WeaponStartLevel;
    public int SecondLevel => WeaponSecondLevel;
    public int ThirdLevel => WeaponThirdLevel;
    public int EndLevel => WeaponEndLevel;
    public int CurrentLevel => _currentLevel;

    [Inject]
    private void Construct(UIServicesProvider UIServices)
    {
        _windowShower = UIServices.PopUpWindow;
    }

    private void Start()
    {
        _currentLevel = WeaponStartLevel;
    }

    public void UpgradeWeapon() //Передать через zenject upgradesystem и weaponupgrader сам будет подписываться на событие?
    {
        if (_currentLevel < WeaponEndLevel)
        {
            _currentLevel++;
            WeaponUpgraded?.Invoke();

            ShowUpgradeMessage();

            if (_currentLevel >= WeaponEndLevel)
            {
                WeaponFullyUpgraded?.Invoke();
                _windowShower.AddMessageToQueue(WeaponFullyUpgradedMessage);
            }
        }
    }

    private void ShowUpgradeMessage()
    {
        switch (_currentLevel)
        {
            case WeaponSecondLevel:
                WeaponSecondLevelUpgraded?.Invoke();
                _windowShower.AddMessageToQueue(WeaponSecondUpgradeMessage);
                break;
            case WeaponThirdLevel:
                WeaponThirdLevelUpgraded?.Invoke();
                _windowShower.AddMessageToQueue(WeaponThirdUpgradeMessage);
                break;
            case WeaponEndLevel:
                _windowShower.AddMessageToQueue(WeaponLastUpgradeMessage);
                break;
        }
    }

    public void SetCurrentLevel(int level)
    {
        if (_currentLevel > WeaponEndLevel || _currentLevel < WeaponStartLevel)
            throw new ArgumentOutOfRangeException(nameof(level));

        for (int i = _currentLevel; i < level; i++)
        {
            WeaponUpgraded?.Invoke();
            ShowUpgradeMessage();
        }

        if (_currentLevel != level)
            _currentLevel = level;


        if (_currentLevel >= WeaponEndLevel)
        {
            WeaponFullyUpgraded?.Invoke();
        }
    }
}