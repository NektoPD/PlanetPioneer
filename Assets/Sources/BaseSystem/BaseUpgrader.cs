using System;
using System.Linq;
using UnityEngine;

public class BaseUpgrader : MonoBehaviour
{
    private const string BaseFullyUpgradedMessage = "Base is already fully upgraded.";
    private const string NoMoreAvailableUnitsMessage = "No more units available to upgrade.";
    private const string BaseUnitUpgradedMessage = "Base Unit upgraded";

    [SerializeField] private UpgradeSystem _upgradeSystem;
    [SerializeField] private UIPopUpWindowShower _windowShower;

    private BaseUnit[] _baseUnits;
    private int _currentUpgrades;
    private int _maximumUpgrades;

    public event Action BaseUpgraded;
    public event Action BaseFullyUpgraded;
    public event Action LoadedBaseUpgrades;

    public int RemainingUpgrades => _currentUpgrades;

    private void Awake()
    {
        _baseUnits = GetComponentsInChildren<BaseUnit>();
        _maximumUpgrades = _baseUnits.Length;
        _currentUpgrades = 0;
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
        if (_currentUpgrades >= _maximumUpgrades)
        {
            _windowShower.AddMessageToQueue(BaseFullyUpgradedMessage);
            return;
        }

        BaseUnit currentUnit = _baseUnits.FirstOrDefault(unit => !unit.gameObject.activeSelf);

        if (currentUnit != null)
        {
            currentUnit.gameObject.SetActive(true);
            _currentUpgrades++;
            BaseUpgraded?.Invoke();

            _windowShower.AddMessageToQueue(BaseUnitUpgradedMessage);

            if (_currentUpgrades >= _maximumUpgrades)
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

    public void SetCurrentUpgrades(int currentUpgrades)
    {
        if (currentUpgrades > _maximumUpgrades || currentUpgrades < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(currentUpgrades));
        }

        UpdateUnitsState(currentUpgrades);

        if (_currentUpgrades != currentUpgrades)
            _currentUpgrades = currentUpgrades;
        
        if (_currentUpgrades >= _maximumUpgrades)
        {
            BaseFullyUpgraded?.Invoke();
        }
    }

    private void UpdateUnitsState(int currentUpgrades)
    {
        for (int i = _currentUpgrades; i < currentUpgrades; i++)
        {
            BaseUnit currentUnit = _baseUnits.FirstOrDefault(unit => !unit.gameObject.activeSelf);

            if (currentUnit == null)
                return;

            currentUnit.gameObject.SetActive(true);
            LoadedBaseUpgrades?.Invoke();
        }
    }
}