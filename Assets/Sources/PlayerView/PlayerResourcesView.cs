using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerResourcesView : MonoBehaviour
{
    [SerializeField] private TMP_Text _ironAmount;
    [SerializeField] private TMP_Text _crystalAmount;
    [SerializeField] private TMP_Text _plantAmount;
    [SerializeField] private TMP_Text _alienArtifactAmount;

    [SerializeField] private TMP_Text _maxIronAmount;
    [SerializeField] private TMP_Text _maxCrystalsAmount;
    [SerializeField] private TMP_Text _maxPlantAmount;
    [SerializeField] private TMP_Text _maxAlienArtifacrtAmount;

    private IResourceHandler _resourceHandler;
    private ICapacityHandler _capacityHandler;
    private Dictionary<Type, TMP_Text> _resourceAmountsTextFields;
    private Dictionary<Type, TMP_Text> _maxResourceAmountsTextFields;

    private void Awake()
    {
        _resourceAmountsTextFields = new Dictionary<Type, TMP_Text>
        {
            { typeof(Iron), _ironAmount },
            { typeof(Crystal), _crystalAmount },
            { typeof(Plant), _plantAmount },
            { typeof(AlienArtifact), _alienArtifactAmount }
        };

        _maxResourceAmountsTextFields = new Dictionary<Type, TMP_Text>
        {
            { typeof(Iron), _maxIronAmount },
            { typeof(Crystal), _maxCrystalsAmount },
            { typeof(Plant), _maxPlantAmount },
            { typeof(AlienArtifact), _maxAlienArtifacrtAmount }
        };
    }
    
    public void SetResourceHandler(IResourceHandler resourceHandler)
    {
        if (resourceHandler == null)
            throw new ArgumentNullException(nameof(resourceHandler));

        _resourceHandler = resourceHandler;

        _resourceHandler.IronAmountChanged += amount => SetResourceAmount(_resourceAmountsTextFields, typeof(Iron), amount);
        _resourceHandler.CrystalAmountChanged +=
            amount => SetResourceAmount(_resourceAmountsTextFields, typeof(Crystal), amount);
        _resourceHandler.PlantAmountChanged += amount => SetResourceAmount(_resourceAmountsTextFields, typeof(Plant), amount);
        _resourceHandler.AlienArtifactAmountChanged +=
            amount => SetResourceAmount(_resourceAmountsTextFields, typeof(AlienArtifact), amount);
    }

    public void SetCapacityHandler(ICapacityHandler capacityHandler)
    {
        if (_resourceHandler == null)
            throw new ArgumentNullException(nameof(capacityHandler));

        _capacityHandler = capacityHandler;
        _capacityHandler.MaxCapacityUpdated += UpdateMaxResourceAmount;
        UpdateMaxResourceAmount();
    }

    private void SetResourceAmount(Dictionary<Type, TMP_Text> dictionary, Type resourceType, int amount)
    {
        if (dictionary.ContainsKey(resourceType))
        {
            dictionary[resourceType].text = amount.ToString();
        }
    }
    
    private void OnDisable()
    {
        _resourceHandler.IronAmountChanged -= amount => SetResourceAmount(_resourceAmountsTextFields, typeof(Iron), amount);
        _resourceHandler.CrystalAmountChanged -=
            amount => SetResourceAmount(_resourceAmountsTextFields, typeof(Crystal), amount);
        _resourceHandler.PlantAmountChanged -= amount => SetResourceAmount(_resourceAmountsTextFields, typeof(Plant), amount);
        _resourceHandler.AlienArtifactAmountChanged -=
            amount => SetResourceAmount(_resourceAmountsTextFields, typeof(AlienArtifact), amount);
        _capacityHandler.MaxCapacityUpdated -= UpdateMaxResourceAmount;
    }

    private void UpdateMaxResourceAmount()
    {
        foreach (var field in _maxResourceAmountsTextFields)
        {
            if (_capacityHandler.MaxCapacityConstaints.TryGetValue(field.Key, out var maxAmount))
            {
                field.Value.text = maxAmount.ToString();
            }
        }
    }
}