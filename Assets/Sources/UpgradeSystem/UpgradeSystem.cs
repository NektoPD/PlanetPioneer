using System;
using UnityEngine;
using YG;
using Zenject;

public class UpgradeSystem : MonoBehaviour, IUpgradeSystem
{
    private const int UpgradeMultiplier = 2;
    private const string DontHaveEnoughGoldToUpgradeError = "Don't have enough gold to upgrade";

    [SerializeField] private UpgradeSystemView _upgradeSystemView;
    [SerializeField] protected UIPopUpWindowShower _windowShower;
    [SerializeField] private SoundPlayer _upgradeSound;
    [SerializeField] private SoundPlayer _errorSound;

    private Player _player;
    private WeaponUpgrader _weaponUpgrader;
    private int _baseUpgradeCost = 50;
    private int _weaponUpgradeCost = 50;
    private int _rocketUpgradeCost = 100;

    public event Action PlayerSteppedIn;
    public event Action PlayerSteppedOut;
    public event Action WeaponUpgraded;
    public event Action BaseUpgraded;
    public event Action RocketUpgraded;
    public event Action RocketUpgradesAvailable;
    public event Action<int> GoldDeducted;
    public event Action<int> WeaponUpgradeCostChanged;
    public event Action<int> BaseUpgradeCostChanged;
    public event Action<int> RocketUpgradeCostChanged;

    public enum UpgradeType
    {
        Weapon,
        Base,
        Rocket
    }

    [Inject]
    private void Construct(Player player)
    {
        _player = player;

        _weaponUpgrader = player.GetComponentInChildren<WeaponUpgrader>();
        _upgradeSystemView.SetWeaponUpgrader(_weaponUpgrader);

        Weapon weapon = _player.GetComponentInChildren<Weapon>();

        if (weapon == null)
            throw new ArgumentNullException(nameof(weapon));

        weapon.SetUpgradeSystem(this);
    }

    private void Start()
    {
        WeaponUpgradeCostChanged?.Invoke(_weaponUpgradeCost);
        BaseUpgradeCostChanged?.Invoke(_baseUpgradeCost);
    }

    private void OnEnable()
    {
        _upgradeSystemView.OnBaseUpgradeButtonClicked += OnBaseUpgradeButtonClicked;
        _upgradeSystemView.OnWeaponUpgradeButtonClicked += OnWeaponUpgradeButtonClicked;
        _upgradeSystemView.OnRocketUpgradeButtonClicked += OnRocketUpgradeButtonClicked;
        _upgradeSystemView.RocketUpgradeButtonEnabled += HandleRocketUpgradesAvailable;
        _upgradeSystemView.WatchAdButtonClicked += HandleWatchAd;
    }

    private void OnDisable()
    {
        _upgradeSystemView.OnBaseUpgradeButtonClicked -= OnBaseUpgradeButtonClicked;
        _upgradeSystemView.OnWeaponUpgradeButtonClicked -= OnWeaponUpgradeButtonClicked;
        _upgradeSystemView.OnRocketUpgradeButtonClicked -= OnRocketUpgradeButtonClicked;
        _upgradeSystemView.RocketUpgradeButtonEnabled -= HandleRocketUpgradesAvailable;
        _upgradeSystemView.WatchAdButtonClicked -= HandleWatchAd;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Player player) && player == _player)
        {
            PlayerSteppedIn?.Invoke();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent(out Player player) && player == _player)
        {
            PlayerSteppedOut?.Invoke();
        }
    }

    private void HandleWatchAd()
    {
        YandexGame.RewVideoShow(0);
    }

    private void OnBaseUpgradeButtonClicked()
    {
        ProcessUpgrade(ref _baseUpgradeCost, BaseUpgraded, BaseUpgradeCostChanged);
    }

    private void OnWeaponUpgradeButtonClicked()
    {
        ProcessUpgrade(ref _weaponUpgradeCost, WeaponUpgraded, WeaponUpgradeCostChanged);
    }

    private void OnRocketUpgradeButtonClicked()
    {
        ProcessUpgrade(ref _rocketUpgradeCost, RocketUpgraded, RocketUpgradeCostChanged);
    }

    private void HandleRocketUpgradesAvailable()
    {
        RocketUpgradesAvailable?.Invoke();
    }

    private void ProcessUpgrade(ref int upgradeCost, Action upgradeAction, Action<int> upgradeCostChangedEvent)
    {
        if (_player.CurrentGoldAmount >= upgradeCost)
        {
            upgradeAction?.Invoke();
            GoldDeducted?.Invoke(upgradeCost);
            upgradeCost = IncreaseUpgradeCost(upgradeCost);
            upgradeCostChangedEvent?.Invoke(upgradeCost);
            _upgradeSound.PlaySound();
        }
        else
        {
            _windowShower.AddMessageToQueue(DontHaveEnoughGoldToUpgradeError);
            _errorSound.PlaySound();
        }
    }

    private int IncreaseUpgradeCost(int upgradeCost)
    {
        return upgradeCost * UpgradeMultiplier;
    }

    public void IncreaseSpecificUpgradeCost(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.Weapon:
                _weaponUpgradeCost = IncreaseUpgradeCost(_weaponUpgradeCost);
                WeaponUpgradeCostChanged?.Invoke(_weaponUpgradeCost);
                break;

            case UpgradeType.Base:
                _baseUpgradeCost = IncreaseUpgradeCost(_baseUpgradeCost);
                BaseUpgradeCostChanged?.Invoke(_baseUpgradeCost);
                break;

            case UpgradeType.Rocket:
                _rocketUpgradeCost = IncreaseUpgradeCost(_rocketUpgradeCost);
                RocketUpgradeCostChanged?.Invoke(_rocketUpgradeCost);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(upgradeType), "Invalid upgrade type");
        }
    }
}