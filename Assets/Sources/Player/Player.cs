using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerUpgrader))]
[RequireComponent(typeof(PlayerGoldHandler))]
[RequireComponent(typeof(PlayerCollisionHandler))]
public class Player : MonoBehaviour,IResourceTaker
{
    [SerializeField] private PlayerResourcesView _resourcesView;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private PlayerGoldView _goldView;
    [SerializeField] private ResourceCatcher _resourceCatcher;
    [SerializeField] private CatchedResourceHandler _resourceHandler;
    
    private UpgradeSystem _upgradeSystem;
    private PlayerCollisionHandler _collisionHandler;
    private PlayerGoldHandler _goldHandler;
    private PlayerUpgrader _upgrader;
    private PlayerMover _mover;
    private Transform _transform;
    
    public event Action<Dictionary<Type, List<Resource>>> ResourcesProvidedToBase;
    public event Action ResourceAddedToBag;
    public event Action ResourceRemovedFromBag;
    public event Action ResourceTaken;

    public int CurrentGoldAmount => _goldHandler.GoldAmount;
    public float CurrentXPosition => _transform.position.x;
    public float CurrentYPosition => _transform.position.y;
    public float CurrentZPosition => _transform.position.z;
    
    [Inject]
    private void Cunstruct(PlanetServicesProvider planetServices)
    {
        _upgradeSystem = planetServices.UpgradeSystem;
    }

    private void Awake()
    {
        _collisionHandler = GetComponent<PlayerCollisionHandler>();
        _goldHandler = GetComponent<PlayerGoldHandler>();
        _upgrader = GetComponent<PlayerUpgrader>();
        _mover = GetComponent<PlayerMover>();
        _transform = transform;
    }

    private void OnEnable()
    {
        _collisionHandler.CollidedWithGold += IncreaseGoldAmount;
        _collisionHandler.CollidedWithTheBase += HandleCollisionWithBase;

        _upgradeSystem.GoldDeducted += _goldHandler.DecreaceGoldAmount;

        _resourceHandler.ResourceAdded += ProcessResourceAddedToBag;
        _resourceHandler.ResourcesCleared += ProcessResourceRemovedFromBag;
    }

    private void OnDisable()
    {
        _collisionHandler.CollidedWithGold -= IncreaseGoldAmount;
        _collisionHandler.CollidedWithTheBase -= HandleCollisionWithBase;

        _upgradeSystem.GoldDeducted -= _goldHandler.DecreaceGoldAmount;

        _resourceHandler.ResourceAdded -= ProcessResourceAddedToBag;
        _resourceHandler.ResourcesCleared -= ProcessResourceRemovedFromBag;
    }

    private void Start()
    {
        _resourcesView.SetResourceHandler(_resourceHandler);
        _goldView.SetGoldHandler(_goldHandler);
        _resourceCatcher.SetPlayerUpgrader(_upgrader);
        _mover.SetPlayerUpgrader(_upgrader);
        _weapon.SetResourceTaker(this);
        _mover.SetResourceGatherer(_weapon);
    }

    public void ProcessGoldIncreaseAmount(int amount)
    {
        _goldHandler.SetGoldAmount(amount);
    }

    public void SetCurrentPosition(Vector3 position)
    {
        _transform.position = position;
    }
    
    private void IncreaseGoldAmount()
    {
        _goldHandler.IncreaseGoldAmount();
    }

    private void HandleCollisionWithBase()
    {
        Dictionary<Type, List<Resource>> resources = _resourceHandler.GetAllResources();
        ResourcesProvidedToBase?.Invoke(resources);
        ResourceTaken?.Invoke();
    }

    private void ProcessResourceAddedToBag()
    {
        ResourceAddedToBag?.Invoke();
    }

    private void ProcessResourceRemovedFromBag()
    {
        ResourceRemovedFromBag?.Invoke();
    }
}
