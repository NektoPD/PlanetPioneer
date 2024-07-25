using System;
using System.Linq;
using UnityEngine;

public class BaseUpgrader : MonoBehaviour
{
    private const string BaseFullyUpgradedMessage = "Base is already fully upgraded.";
    private const string NoMoreAvailableUnitsMessage = "No more units available to upgrade.";

    [SerializeField] private UpgradeSystem _upgradeSystem;
    [SerializeField] private UIPopUpWindowShower _windowShower;

    private BaseUnit[] _baseUnits;
    private int _remainingUpgrades;

    public event Action BaseUpgraded;
    public event Action BaseFullyUpgraded;

    private void Awake()
    {
        _baseUnits = GetComponentsInChildren<BaseUnit>();
        _remainingUpgrades = _baseUnits.Length;
    }

    private void OnEnable()
    {
        _upgradeSystem.BaseUpgraded += UpgradeOneUnit;
    }

    private void OnDisable()
    {
        _upgradeSystem.BaseUpgraded -= UpgradeOneUnit;
    }

    private void Start()
    {
        foreach (var unit in _baseUnits)
        {
            unit.gameObject.SetActive(false);
        }
    }

    private void UpgradeOneUnit()
    {
        if (_remainingUpgrades <= 0)
        {
            _windowShower.AddMessageToQueue(BaseFullyUpgradedMessage);
            return;
        }

        BaseUnit currentUnit = _baseUnits.FirstOrDefault(unit => !unit.gameObject.activeSelf);

        if (currentUnit != null)
        {
            currentUnit.gameObject.SetActive(true);
            _remainingUpgrades--;
            BaseUpgraded?.Invoke();

            _windowShower.AddMessageToQueue($"Unit upgraded. {_remainingUpgrades} upgrades remaining.");

            if (_remainingUpgrades == 0)
            {
                _windowShower.AddMessageToQueue(BaseFullyUpgradedMessage);
                BaseFullyUpgraded?.Invoke();
            }
        }
        else
        {
            _windowShower.AddMessageToQueue(NoMoreAvailableUnitsMessage);
        }
    }
}

