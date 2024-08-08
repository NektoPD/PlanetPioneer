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

    private CatchedResourceHandler _resourceHandler;
    private Dictionary<Type, TMP_Text> _resourceTextFields;
    private Dictionary<Type, TMP_Text> _maxResourceTextFields;

    private void Awake()
    {
        _resourceTextFields = new Dictionary<Type, TMP_Text>
        {
            { typeof(Iron), _ironAmount },
            { typeof(Crystal), _crystalAmount },
            { typeof(Plant), _plantAmount },
            { typeof(AlienArtifact), _alienArtifactAmount }
        };

        _maxResourceTextFields = new Dictionary<Type, TMP_Text>
        {
            {typeof(Iron), _maxIronAmount },
            { typeof(Crystal), _maxCrystalsAmount },
            { typeof(Plant), _maxPlantAmount },
            { typeof(AlienArtifact), _maxAlienArtifacrtAmount }
        };
    }

    private void SetResourceAmount(Dictionary<Type, TMP_Text> dictionary, Type resourceType, int amount)
    {
        if (dictionary.ContainsKey(resourceType))
        {
            dictionary[resourceType].text = amount.ToString();
        }
    }

    public void SetResourceHandler(CatchedResourceHandler resourceHandler)
    {
        if (resourceHandler == null)
            throw new ArgumentNullException();

        _resourceHandler = resourceHandler;

        _resourceHandler.IronAmountChanged += amount => SetResourceAmount(_resourceTextFields,typeof(Iron), amount);
        _resourceHandler.CrystalAmountChanged += amount => SetResourceAmount(_resourceTextFields, typeof(Crystal), amount);
        _resourceHandler.PlantAmountChanged += amount => SetResourceAmount(_resourceTextFields, typeof(Plant), amount);
        _resourceHandler.AlienArtifactAmountChanged += amount => SetResourceAmount(_resourceTextFields, typeof(AlienArtifact), amount);
        _resourceHandler.MaxCapacityUpgraded += UpdateMaxResourceAmount;
        
        UpdateMaxResourceAmount();
    }

    private void OnDisable()
    {
        if (_resourceHandler != null)
        {
            _resourceHandler.IronAmountChanged -= amount => SetResourceAmount(_resourceTextFields, typeof(Iron), amount);
            _resourceHandler.CrystalAmountChanged -= amount => SetResourceAmount(_resourceTextFields,typeof(Crystal), amount);
            _resourceHandler.PlantAmountChanged -= amount => SetResourceAmount(_resourceTextFields, typeof(Plant), amount);
            _resourceHandler.AlienArtifactAmountChanged -= amount => SetResourceAmount(_resourceTextFields, typeof(AlienArtifact), amount);
            _resourceHandler.MaxCapacityUpgraded -= UpdateMaxResourceAmount;
        }
    }

    private void UpdateMaxResourceAmount()
    {
        foreach (var field in _maxResourceTextFields)
        {
            if (_resourceHandler.MaxCapacityConstaints.TryGetValue(field.Key, out var maxAmount))
            {
                field.Value.text = maxAmount.ToString();
            }
        }
    }
}
