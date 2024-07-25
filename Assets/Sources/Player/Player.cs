using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerUpgrader))]
[RequireComponent(typeof(PlayerGoldHandler))]
[RequireComponent(typeof(PlayerCollisionHandler))]
public class Player : MonoBehaviour
{
    [SerializeField] private UpgradeSystem _upgradeSystem;
    [SerializeField] private PlayerToBasePointerView _basePointerView;
    [SerializeField] private BaseUpgrader _baseUpgrader;

    private CatchedResourceHandler _resourceHandler;
    private PlayerResourcesVisabiltyView _resourcesView;
    private PlayerCollisionHandler _collisionHandler;
    private PlayerGoldHandler _goldHandler;
    private PlayerGoldView _goldView;
    private PlayerUpgrader _upgrader;
    private PlayerMover _mover;
    private Weapon _weapon;
    private ResourceCatcher _resourceCatcher;

    public int CurrentGoldAmount => _goldHandler.GoldAmount;

    public event Action<Dictionary<Type, List<Resource>>> ResourcesProvidedToBase;
    public event Action ResourceAddedToBag;
    public event Action ResourceRemovedFromBag;
    public event Action BaseUpgraded;

    private void Awake()
    {
        _resourceHandler = GetComponentInChildren<CatchedResourceHandler>();
        _resourcesView = GetComponentInChildren<PlayerResourcesVisabiltyView>();
        _collisionHandler = GetComponent<PlayerCollisionHandler>();
        _goldHandler = GetComponent<PlayerGoldHandler>();
        _goldView = GetComponentInChildren<PlayerGoldView>();
        _upgrader = GetComponent<PlayerUpgrader>();
        _mover = GetComponent<PlayerMover>();
        _weapon = GetComponentInChildren<Weapon>();
        _resourceCatcher = GetComponentInChildren<ResourceCatcher>();
    }

    private void OnEnable()
    {
        _collisionHandler.CollidedWithGold += IncreaseGoldAmount;
        _collisionHandler.CollidedWithTheBase += HandleCollisionWithBase;
        _upgradeSystem.GoldDeducted += _goldHandler.DecreaceGoldAmount;
        _upgradeSystem.BaseUpgraded += ProcessPlayerUpgrade;

        _upgrader.UpgradedBag += ProcessMaxResourceBagUpgrade;

        _weapon.StartedGatheringResources += _mover.DisableMovement;
        _weapon.StopedGatheringResources += _mover.EnableMovement;

        _resourceHandler.ResourceAdded += ProcessResourceAddedToBag;
        _resourceHandler.ResourcesCleared += ProcessResourceRemovedFromBag;

        _baseUpgrader.BaseUpgraded += ProcessBaseUpgrade;
    }

    private void OnDisable()
    {
        _collisionHandler.CollidedWithGold -= IncreaseGoldAmount;
        _collisionHandler.CollidedWithTheBase -= HandleCollisionWithBase;
        _upgradeSystem.GoldDeducted -= _goldHandler.DecreaceGoldAmount;
        _upgradeSystem.BaseUpgraded -= ProcessPlayerUpgrade;

        _upgrader.UpgradedBag -= ProcessMaxResourceBagUpgrade;

        _weapon.StartedGatheringResources -= _mover.DisableMovement;
        _weapon.StopedGatheringResources -= _mover.EnableMovement;

        _resourceHandler.ResourceAdded -= ProcessResourceAddedToBag;
        _resourceHandler.ResourcesCleared -= ProcessResourceRemovedFromBag;

        _baseUpgrader.BaseUpgraded -= ProcessBaseUpgrade;
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

    private void IncreaseGoldAmount()
    {
        _goldHandler.SetGoldAmount();
    }

    private void HandleCollisionWithBase()
    {
        Dictionary<Type, List<Resource>> resources = _resourceHandler.GetAllResources();
        ResourcesProvidedToBase?.Invoke(resources);
        _resourceHandler.ClearAllResources();
    }

    private void ProcessPlayerUpgrade()
    {
        _upgrader.ProcessUpgrade();
    }

    private void ProcessMaxResourceBagUpgrade()
    {
        _resourceHandler.UpgradeMaxResourceCapacity();
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
}
