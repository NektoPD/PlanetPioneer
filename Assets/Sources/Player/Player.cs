using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerUpgrader))]
[RequireComponent(typeof(PlayerGoldHandler))]
[RequireComponent(typeof(PlayerCollisionHandler))]
public class Player : MonoBehaviour
{
    private UpgradeSystem _upgradeSystem;
    private PlayerToBasePointerView _basePointerView;
    private BaseUpgrader _baseUpgrader;
    private CatchedResourceHandler _resourceHandler;
    private PlayerResourcesView _resourcesView;
    private PlayerCollisionHandler _collisionHandler;
    private PlayerGoldHandler _goldHandler;
    private PlayerGoldView _goldView;
    private PlayerUpgrader _upgrader;
    private PlayerMover _mover;
    private Weapon _weapon;
    private ResourceCatcher _resourceCatcher;
    private Transform _transform;

    public int CurrentGoldAmount => _goldHandler.GoldAmount;

    public event Action<Dictionary<Type, List<Resource>>> ResourcesProvidedToBase;
    public event Action ResourceAddedToBag;
    public event Action ResourceRemovedFromBag;
    public event Action BaseUpgraded;

    public float CurrentXPosition => _transform.position.x;
    public float CurrentYPosition => _transform.position.y;
    public float CurrentZPosition => _transform.position.z;


    [Inject]
    private void Cunstruct(PlanetServicesProvider planetServices)
    {
        _upgradeSystem = planetServices.UpgradeSystem;
        _basePointerView = planetServices.BasePointerView;
        _baseUpgrader = planetServices.BaseUpgrader;
    }

    private void Awake()
    {
        _resourceHandler = GetComponentInChildren<CatchedResourceHandler>();
        _resourcesView = GetComponentInChildren<PlayerResourcesView>();
        _collisionHandler = GetComponent<PlayerCollisionHandler>();
        _goldHandler = GetComponent<PlayerGoldHandler>();
        _goldView = GetComponentInChildren<PlayerGoldView>();
        _upgrader = GetComponent<PlayerUpgrader>();
        _mover = GetComponent<PlayerMover>();
        _weapon = GetComponentInChildren<Weapon>();
        _resourceCatcher = GetComponentInChildren<ResourceCatcher>();
        _transform = transform;
    }

    private void OnEnable()
    {
        _collisionHandler.CollidedWithGold += IncreaseGoldAmount;
        _collisionHandler.CollidedWithTheBase += HandleCollisionWithBase;

        _upgradeSystem.GoldDeducted += _goldHandler.DecreaceGoldAmount;
        _upgradeSystem.BaseUpgraded += ProcessPlayerUpgrade;

        _weapon.StartedGatheringResources += _mover.DisableMovement;
        _weapon.StopedGatheringResources += _mover.EnableMovement;

        _resourceHandler.ResourceAdded += ProcessResourceAddedToBag;
        _resourceHandler.ResourcesCleared += ProcessResourceRemovedFromBag;

        _baseUpgrader.BaseUpgraded += ProcessBaseUpgrade;
        _baseUpgrader.LoadedBaseUpgrades += ProcessBaseUpgrade;
    }

    private void OnDisable()
    {
        _collisionHandler.CollidedWithGold -= IncreaseGoldAmount;
        _collisionHandler.CollidedWithTheBase -= HandleCollisionWithBase;

        _upgradeSystem.GoldDeducted -= _goldHandler.DecreaceGoldAmount;
        _upgradeSystem.BaseUpgraded -= ProcessPlayerUpgrade;
        
        _weapon.StartedGatheringResources -= _mover.DisableMovement;
        _weapon.StopedGatheringResources -= _mover.EnableMovement;

        _resourceHandler.ResourceAdded -= ProcessResourceAddedToBag;
        _resourceHandler.ResourcesCleared -= ProcessResourceRemovedFromBag;

        _baseUpgrader.BaseUpgraded -= ProcessBaseUpgrade;
        _baseUpgrader.LoadedBaseUpgrades -= ProcessBaseUpgrade;
    }

    private void Start()
    {
        _basePointerView.SetPlayer(this);
        _upgrader.SetPlayer(this);
        _resourcesView.SetResourceHandler(_resourceHandler);
        _goldView.SetGoldHandler(_goldHandler);
        _resourceHandler.SetPlayerUpgrader(_upgrader);
        _resourceCatcher.SetPlayerUpgrader(_upgrader);
        _mover.SetPlayerUpgrader(_upgrader);
    }

    public void ProcessGoldIncreaseAmount(int amount)
    {
        _goldHandler.SetGoldAmount(amount);
    }

    private void IncreaseGoldAmount()
    {
        _goldHandler.IncreaseGoldAmount();
    }

    private void HandleCollisionWithBase()
    {
        Dictionary<Type, List<Resource>> resources = _resourceHandler.GetAllResources();
        ResourcesProvidedToBase?.Invoke(resources);
    }

    private void ProcessPlayerUpgrade()
    {
        _upgrader.ProcessUpgrade();
    }

    private void ProcessResourceAddedToBag()
    {
        ResourceAddedToBag?.Invoke();
    }

    private void ProcessResourceRemovedFromBag()
    {
        ResourceRemovedFromBag?.Invoke();
    }

    private void ProcessBaseUpgrade()
    {
        BaseUpgraded?.Invoke();
    }

    public void SetCurrentPosition(Vector3 position)
    {
        _transform.position = position;
    }
}
