using System;
using UnityEngine;

[RequireComponent(typeof(WeaponLevelChecker))]
[RequireComponent(typeof(ResourceCatcher))]
[RequireComponent(typeof(CatchedResourceHandler))]
[RequireComponent(typeof(WeaponUpgrader))]
[RequireComponent(typeof(ParticleSpawner))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private SoundController _weaponSound;
    
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

    public ResourceCatcher ResourceCatcher => _resourceCatcher;

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
    }

    private void Start()
    {
        _resourceCatcher.SetWeapon(this);
        _resourceCatcher.SetCapacityChecker(_resourceCatcherHandler);
        _resourceCatcherHandler.SetWeapon(this);
        _playerInput.Player.Gather.performed += ctx => ShootButtonPressed?.Invoke();

        _weaponLevelChecker.SetWeaponUpgrader(_upgrader);
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _resourceCatcher.StartedGatheringResources -= ProcessResourceGatherStart;
        _resourceCatcher.StoppedGatheringResources -= ProcessResourceGatherEnd;
        _upgradeSystem.WeaponUpgraded -= _upgrader.UpgradeWeapon;
    }

    public bool CanCollectResource(Resource resource)
    {
        return _weaponLevelChecker.IsWeaponLevelSufficient(resource);
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
}
