using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(WeaponLevelChecker))]
[RequireComponent(typeof(ResourceCatcher))]
[RequireComponent(typeof(CatchedResourceHandler))]
[RequireComponent(typeof(WeaponUpgrader))]
[RequireComponent(typeof(ParticleSpawner))]
public class Weapon : MonoBehaviour,IShooter,IResourceGatherer
{
    [SerializeField] private SoundPlayer _weaponSound;
    [SerializeField] private PlayerUpgrader _playerUpgrader;
    [SerializeField] private PlayerResourcesView _resourceView;
    [SerializeField] private Button _shootButton;

    private PlayerInput _playerInput;
    private ResourceCatcher _resourceCatcher;
    private CatchedResourceHandler _resourceCatcherHandler;
    private WeaponUpgrader _upgrader;
    private IUpgradeSystem _upgradeSystem;
    private WeaponLevelChecker _weaponLevelChecker;
    private ParticleSpawner _particleSpawner;

    public event Action ShootButtonPressed;
    public event Action StartedGatheringResources;
    public event Action StopedGatheringResources;

    private void Awake()
    {
        _resourceCatcher = GetComponent<ResourceCatcher>();
        _resourceCatcherHandler = GetComponent<CatchedResourceHandler>();
        _upgrader = GetComponent<WeaponUpgrader>();
        _weaponLevelChecker = GetComponent<WeaponLevelChecker>();
        _particleSpawner = GetComponent<ParticleSpawner>();

        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();

        _resourceCatcher.StartedGatheringResources += ProcessResourceGatherStart;
        _resourceCatcher.StoppedGatheringResources += ProcessResourceGatherEnd;
        _shootButton.onClick.AddListener(OnShootButtonPressed);
    }

    private void Start()
    {
        _resourceCatcher.SetShooter(this);
        _resourceCatcher.SetCapacityChecker(_resourceCatcherHandler);
        _resourceCatcher.SetWeaponLevelChecker(_weaponLevelChecker);
        _resourceCatcher.SetPlayerUpgrader(_playerUpgrader);
        
        _resourceCatcherHandler.SetResourceCatcher(_resourceCatcher);
        _resourceCatcherHandler.SetPlayerUpgrader(_playerUpgrader);
        _weaponLevelChecker.SetWeaponUpgrader(_upgrader);
        _resourceView.SetCapacityHandler(_resourceCatcherHandler);
        
        _playerInput.Player.Gather.performed += ctx => ShootButtonPressed?.Invoke();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _resourceCatcher.StartedGatheringResources -= ProcessResourceGatherStart;
        _resourceCatcher.StoppedGatheringResources -= ProcessResourceGatherEnd;
        _upgradeSystem.WeaponUpgraded -= _upgrader.UpgradeWeapon;
        _shootButton.onClick.RemoveListener(OnShootButtonPressed);
    }

    public void SetUpgradeSystem(IUpgradeSystem upgradeSystem)
    {
        if (upgradeSystem == null)
            throw new ArgumentNullException(nameof(upgradeSystem));

        if (_upgradeSystem != null)
            return;

        _upgradeSystem = upgradeSystem;
        _upgradeSystem.WeaponUpgraded += _upgrader.UpgradeWeapon;
    }

    public void SetResourceTaker(IResourceTaker resourceTaker)
    {
        if (resourceTaker == null)
            throw new ArgumentNullException(nameof(resourceTaker));
        
        _resourceCatcherHandler.SetResourceTaker(resourceTaker);
    }

    private void ProcessResourceGatherStart()
    {
        StartedGatheringResources?.Invoke();
        _particleSpawner.ActivateParticle();
        _weaponSound.PlaySound();
    }

    private void ProcessResourceGatherEnd()
    {
        StopedGatheringResources?.Invoke();
        _particleSpawner.DeactivateParticle();
        _weaponSound.StopPlayingSound();
    }

    private void OnShootButtonPressed()
    {
        ShootButtonPressed?.Invoke();
    }
}
