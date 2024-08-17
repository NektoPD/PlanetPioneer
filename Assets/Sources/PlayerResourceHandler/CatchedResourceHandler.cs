using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class CatchedResourceHandler : MonoBehaviour, ICapacityHandler,IResourceHandler
{
    private IResourceCatcher _resourceCatcher;
    private IPlayerUpgrader _playerUpgrader;
    private IResourceTaker _resourceTaker;
    
    private int _startIronCapacity = 15;
    private int _startCrystalCapacity = 10;
    private int _startPlantCapacity = 5;
    private int _startAlienArtifactCapacity = 2;

    private int _upgradedMaxIronCapacity = 20;
    private int _upgradedMaxCrystalCapacity = 15;
    private int _upgradedMaxPlantCapacity = 8;
    private int _upgradedAlienArtifactCapacity = 3;

    private Dictionary<Type, int> _resources = new Dictionary<Type, int>();
    private Dictionary<Type, int> _maxCapacityConstraints = new Dictionary<Type, int>();

    public event Action ResourceAdded;
    public event Action ResourceAmountChanged;
    public event Action ResourcesCleared;
    public event Action MaxCapacityUpdated;

    public IReadOnlyDictionary<Type, int> CurrentResourceCatched => _resources;
    public IReadOnlyDictionary<Type, int> MaxCapacityConstaints => _maxCapacityConstraints;
    /*public List<Resource> CurrentIronAmount => _resources.GetValueOrDefault(typeof(Iron));
    public List<Resource> CurrentCrystalAmount => _resources.GetValueOrDefault(typeof(Crystal));
    public List<Resource> CurrentPlantAmount => _resources.GetValueOrDefault(typeof(Plant));
    public List<Resource> CurrentAlienArtifactAmount => _resources.GetValueOrDefault(typeof(AlienArtifact));*/
    
    
    private void Start()
    { 
        _maxCapacityConstraints[typeof(Iron)] = _startIronCapacity;
        _maxCapacityConstraints[typeof(Crystal)] = _startCrystalCapacity;
        _maxCapacityConstraints[typeof(Plant)] = _startPlantCapacity;
        _maxCapacityConstraints[typeof(AlienArtifact)] = _startAlienArtifactCapacity;
    }

    private void OnDisable()
    {
        _resourceCatcher.CatchedResource -= AddResource;
        _playerUpgrader.UpgradedBag -= UpgradeMaxResourceCapacity;
        _resourceTaker.ResourceTaken -= ClearAllResources;
    }

    public void SetResourceCatcher(IResourceCatcher resourceCatcher)
    {
        if (resourceCatcher == null)
            throw new ArgumentNullException(nameof(resourceCatcher));

        _resourceCatcher = resourceCatcher;
        _resourceCatcher.CatchedResource += AddResource;
    }

    public void SetPlayerUpgrader(IPlayerUpgrader playerUpgrader)
    {
        if (playerUpgrader == null)
            throw new ArgumentNullException(nameof(playerUpgrader));

        _playerUpgrader = playerUpgrader;
        _playerUpgrader.UpgradedBag += UpgradeMaxResourceCapacity;
    }

    public void SetResourceTaker(IResourceTaker resourceTaker)
    {
        _resourceTaker = resourceTaker;
        _resourceTaker.ResourceTaken += ClearAllResources;
    }
    
    public bool IsMaxCapacityReached(Type resourceType)
    {
        if (_resources.ContainsKey(resourceType))
        {
            return _resources[resourceType] >= _maxCapacityConstraints[resourceType];
        }
        return false;
    }
    
    public Dictionary<Type, int> GetAllResources()
    {
        return new Dictionary<Type, int>(_resources);
    }
    
    private void AddResource(Resource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));

        Type resourceType = resource.GetType();

        if (!_resources.ContainsKey(resourceType))
            _resources[resourceType] = 0;

        _resources[resourceType]++;
        ResourceAdded?.Invoke();
        ResourceAmountChanged?.Invoke();
    }

    private void ClearAllResources()
    { 
        _resources.Clear();
        
        ResourceAmountChanged?.Invoke();
        ResourcesCleared?.Invoke();
    }

    private void UpgradeMaxResourceCapacity()
    {
        _maxCapacityConstraints[typeof(Iron)] = _upgradedMaxIronCapacity;
        _maxCapacityConstraints[typeof(Crystal)] = _upgradedMaxCrystalCapacity;
        _maxCapacityConstraints[typeof(Plant)] = _upgradedMaxPlantCapacity;
        _maxCapacityConstraints[typeof(AlienArtifact)] = _upgradedAlienArtifactCapacity;
        MaxCapacityUpdated?.Invoke();
    }
    
    public void SetResourceAmount(Dictionary<Type, int> resources)
    {
        foreach (var resource in resources)
        {
            if (_resources.ContainsKey(resource.Key))
            {
                _resources[resource.Key] = resource.Value;
            }
        }
        
        ResourceAmountChanged?.Invoke();
    }
}