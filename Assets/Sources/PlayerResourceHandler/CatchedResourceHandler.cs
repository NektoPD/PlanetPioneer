using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject.ReflectionBaking.Mono.Cecil;

public class CatchedResourceHandler : MonoBehaviour, ICapacityHandler
{
    private int _startIronCapacity = 15;
    private int _startCrystalCapacity = 10;
    private int _startPlantCapacity = 5;
    private int _startAlienArtifactCapacity = 2;

    private int _upgradedMaxIronCapacity = 20;
    private int _upgradedMaxCrystalCapacity = 15;
    private int _upgradedMaxPlantCapacity = 8;
    private int _upgradedAlienArtifactCapacity = 3;

    private Dictionary<Type, List<Resource>> _resources = new Dictionary<Type, List<Resource>>();
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
    
    public List<Resource> CurrentIronAmount => _resources.GetValueOrDefault(typeof(Iron));
    public List<Resource> CurrentCrystalAmount => _resources.GetValueOrDefault(typeof(Crystal));
    public List<Resource> CurrentPlantAmount => _resources.GetValueOrDefault(typeof(Plant));
    public List<Resource> CurrentAlienArtifactAmount => _resources.GetValueOrDefault(typeof(AlienArtifact));
    
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
        var dictionaryToReturn = new Dictionary<Type, List<Resource>>(_resources);
        ClearAllResources();
        return dictionaryToReturn;
    }

    private void ClearAllResources()
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

    private void UpgradeMaxResourceCapacity()
    {
        _maxCapacityConstraints[typeof(Iron)] = _upgradedMaxIronCapacity;
        _maxCapacityConstraints[typeof(Crystal)] = _upgradedMaxCrystalCapacity;
        _maxCapacityConstraints[typeof(Plant)] = _upgradedMaxPlantCapacity;
        _maxCapacityConstraints[typeof(AlienArtifact)] = _upgradedAlienArtifactCapacity;

        MaxCapacityUpgraded?.Invoke();
    }

    private void ChangeResourceCount(Type resourceType)
    {
        if (resourceType == typeof(Iron))
            IronAmountChanged?.Invoke(_resources[resourceType].Count);
        else if (resourceType == typeof(Crystal))
            CrystalAmountChanged?.Invoke(_resources[resourceType].Count);
        else if (resourceType == typeof(Plant))
            PlantAmountChanged?.Invoke(_resources[resourceType].Count);
        else if (resourceType == typeof(AlienArtifact))
            AlienArtifactAmountChanged?.Invoke(_resources[resourceType].Count);
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

    public void SetResourceAmount(List<Resource> resourcesToAdd)
    {
        if(resourcesToAdd == null)
            throw new ArgumentOutOfRangeException(nameof(resourcesToAdd));
        
        if(resourcesToAdd.Count == 0)
            return;

        Type resourceToAddType = resourcesToAdd.First().GetType();
        
        if (!_resources.ContainsKey(resourceToAddType))
            _resources[resourceToAddType] = new List<Resource>();

        _resources[resourceToAddType].Clear();
        _resources[resourceToAddType].AddRange(resourcesToAdd);

        ChangeResourceCount(resourceToAddType);
    }
}