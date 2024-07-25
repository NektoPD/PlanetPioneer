using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CatchedResourceHandler : MonoBehaviour, ICapacityChecker
{
    private int _startIronCapacity = 15;
    private int _startCrystalCapacity = 10;
    private int _startPlantCapacity = 5;
    private int _startAlienArtifactCapacity = 2;

    private int _upgradedMaxIronCapacity = 20;
    private int _upgradedMaxCrystalCapacity = 15; //Пересмотреть
    private int _upgradedMaxPlantCapacity = 8; //Пересмотреть
    private int _upgradedAlienArtifactCapacity = 3; //Пересмотреть

    private readonly Dictionary<Type, List<Resource>> _resources = new Dictionary<Type, List<Resource>>();
    private Dictionary<Type, int> _maxCapacityConstraints = new Dictionary<Type, int>();

    private Weapon _weapon;
    private PlayerUpgrader _playerUpgrader;

    public IReadOnlyDictionary<Type, int> MaxCapacityConstaints => _maxCapacityConstraints;

    public event Action ResourceAdded;
    public event Action ResourcesCleared;
    public event Action MaxCapacityUpgraded;

    public event Action<int> IronAmountChanged;
    public event Action<int> CrystalAmountChanged;
    public event Action<int> PlantAmountChanged;
    public event Action<int> AlienArtifactAmountChanged;

    private void Start()
    {
        _resources[typeof(Iron)] = new List<Resource>();
        _resources[typeof(Crystal)] = new List<Resource>();
        _resources[typeof(Plant)] = new List<Resource>();
        _resources[typeof(AlienArtifact)] = new List<Resource>();

        _maxCapacityConstraints[typeof(Iron)] = _startIronCapacity;
        _maxCapacityConstraints[typeof(Crystal)] = _startCrystalCapacity;
        _maxCapacityConstraints[typeof(Plant)] = _startPlantCapacity;
        _maxCapacityConstraints[typeof(AlienArtifact)] = _startAlienArtifactCapacity;
    }

    public void AddResource(Resource resource)
    {
        if (resource == null)
            throw new ArgumentNullException();

        Type resourceType = resource.GetType();

        if (!_resources.ContainsKey(resourceType))
            _resources[resourceType] = new List<Resource>();

        _resources[resourceType].Add(resource);
        ResourceAdded?.Invoke();
        ChangeResourceCount(resourceType);
    }

    public bool IsMaxCapacityReached(Type resourceType)
    {
        if (_resources.ContainsKey(resourceType))
        {
            return _resources[resourceType].Count >= _maxCapacityConstraints[resourceType];
        }
        return false;
    }

    public Dictionary<Type, List<Resource>> GetAllResources()
    {
        return new Dictionary<Type, List<Resource>>(_resources);
    }

    public void ClearAllResources()
    {
        foreach (var resourceType in _resources.Keys)
        {
            _resources[resourceType].Clear();
            ChangeResourceCount(resourceType);
        }

        ResourcesCleared?.Invoke();
    }

    public void SetWeapon(Weapon weapon)
    {
        if (weapon == null)
            throw new ArgumentNullException();

        _weapon = weapon;
        _weapon.ResourceCatcher.CatchedResource += AddResource;
    }

    public void UpgradeMaxResourceCapacity()
    {
        _maxCapacityConstraints[typeof(Iron)] = _upgradedMaxIronCapacity;
        _maxCapacityConstraints[typeof(Crystal)] = _upgradedMaxCrystalCapacity;
        _maxCapacityConstraints[typeof(Plant)] = _upgradedMaxPlantCapacity;
        _maxCapacityConstraints[typeof(AlienArtifact)] = _upgradedAlienArtifactCapacity;

        MaxCapacityUpgraded?.Invoke();
    }

    private void ChangeResourceCount(Type resourceType)
    {
        switch (resourceType)
        {
            case Type t when t == typeof(Iron):
                IronAmountChanged?.Invoke(_resources[resourceType].Count);
                break;
            case Type t when t == typeof(Crystal):
                CrystalAmountChanged?.Invoke(_resources[resourceType].Count);
                break;
            case Type t when t == typeof(Plant):
                PlantAmountChanged?.Invoke(_resources[resourceType].Count);
                break;
            case Type t when t == typeof(AlienArtifact):
                AlienArtifactAmountChanged?.Invoke(_resources[resourceType].Count);
                break;
        }
    }

    public void SetPlayerUpgrader(PlayerUpgrader upgrader)
    {
        if(upgrader == null) 
            throw new ArgumentNullException(nameof(upgrader));

        _playerUpgrader = upgrader;
        _playerUpgrader.UpgradedBag += UpgradeMaxResourceCapacity;
    }

    private void OnDisable()
    {
        _weapon.ResourceCatcher.CatchedResource -= AddResource;
        _playerUpgrader.UpgradedBag -= UpgradeMaxResourceCapacity;
    }
}


