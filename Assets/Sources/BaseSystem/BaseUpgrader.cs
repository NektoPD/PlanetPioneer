using System;
using System.Linq;
using UnityEngine;
using YG;

public class BaseUpgrader : MonoBehaviour
{
    private const string BaseFullyUpgradedMessage = "Base is already fully upgraded.";
    private const string NoMoreAvailableUnitsMessage = "No more units available to upgrade.";
    private const string BaseUnitUpgradedMessage = "Base Unit upgraded";

    [SerializeField] private UpgradeSystem _upgradeSystem;
    [SerializeField] private UIPopUpWindowShower _windowShower;
    [SerializeField] private BaseUnit[] _baseUnits;

    private int _currentUpgrades;
    private int _maximumUpgrades;

    public event Action BaseUpgraded;
    public event Action BaseFullyUpgraded;
    public event Action LoadedBaseUpgrades;

    public int RemainingUpgrades => _currentUpgrades;

    private void Awake()
    {
        foreach (var unit in _baseUnits)
        {
            unit.gameObject.SetActive(false);
        }
        
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

    public void SetCurrentUpgrades(int currentUpgrades)
    {
        if (currentUpgrades > _maximumUpgrades || currentUpgrades < 0)
            throw new ArgumentOutOfRangeException(nameof(currentUpgrades));

        _currentUpgrades = currentUpgrades;
        UpdateUnitsState();

        if (_currentUpgrades >= _maximumUpgrades)
        {
            BaseFullyUpgraded?.Invoke();
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

            YandexGame.FullscreenShow();
        }
        else
        {
            _windowShower.AddMessageToQueue(NoMoreAvailableUnitsMessage);
        }
    }

    private void UpdateUnitsState()
    {
        for (int i = 0; i < _currentUpgrades; i++)
        {
            BaseUnit currentUnit = _baseUnits.FirstOrDefault(unit => !unit.gameObject.activeSelf);

            if (currentUnit == null)
            {
                throw new ArgumentNullException(nameof(currentUnit));
            }

            currentUnit.gameObject.SetActive(true);
            LoadedBaseUpgrades?.Invoke();
            _upgradeSystem.IncreaseSpecificUpgradeCost(UpgradeType.Base);
        }
    }
}