using System;
using UnityEngine;
using Zenject;

public class PlayerUpgrader : MonoBehaviour, IPlayerUpgrader
{
    private const string PlayerBagUpgradedMessage = "Players bag is now upgraded";
    private const string PlayerGatherSpeedUpgradedMessage = "Player gather speed is now upgraded";
    private const string PlayerMovingSpeedUpgradedMessage = "Player moving speed is now upgraded";
    private const string PlayerGatherRadiusUpgradedMessage = "Player gather speed in now upgraded";

    private BaseUpgrader _baseUpgrader;
    private UIPopUpWindowShower _windowShower;
    private const int MaxPossibleUpgrades = 4;
    private int _currentUpgradeLevel = 0;

    public event Action UpgradedBag;
    public event Action UpgradedGatherSpeed;
    public event Action UpgradedPlayerMovingSpeed;
    public event Action UpgradedGatherRadius;

    [Inject]
    private void Construct(UIServicesProvider UIServices, PlanetServicesProvider planetServices)
    {
        _windowShower = UIServices.PopUpWindow;
        _baseUpgrader = planetServices.BaseUpgrader;
        _baseUpgrader.BaseUpgraded += ProcessUpgrade;
        _baseUpgrader.LoadedBaseUpgrades += ProcessUpgrade;
    }

    private void OnDisable()
    {
        _baseUpgrader.BaseUpgraded -= ProcessUpgrade;
        _baseUpgrader.LoadedBaseUpgrades -= ProcessUpgrade;
    }

    public void ProcessUpgrade()
    {
        if (_currentUpgradeLevel < MaxPossibleUpgrades)
        {
            _currentUpgradeLevel++;
            ApplyPlayerUpgrade(_currentUpgradeLevel);
        }
    }

    private void ApplyPlayerUpgrade(int upgradeLevel)
    {
        switch (upgradeLevel)
        {
            case 1:
                _windowShower.AddMessageToQueue(PlayerBagUpgradedMessage);
                UpgradedBag?.Invoke();
                break;
            case 2:
                _windowShower.AddMessageToQueue(PlayerGatherSpeedUpgradedMessage);
                UpgradedGatherSpeed?.Invoke();
                break;
            case 3:
                _windowShower.AddMessageToQueue(PlayerMovingSpeedUpgradedMessage);
                UpgradedPlayerMovingSpeed?.Invoke();
                break;
            case 4:
                _windowShower.AddMessageToQueue(PlayerGatherRadiusUpgradedMessage);
                UpgradedGatherRadius?.Invoke();
                break;
        }
    }
}