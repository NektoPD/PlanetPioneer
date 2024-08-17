using System;
using UnityEngine;
using Zenject;

public class GameSaver : MonoBehaviour
{
    private BaseUpgrader _baseUpgrader;
    private RocketBuilder _rocketBuilder;
    private IWeaponUpgrader _weaponUpgrader;
    private IResourceHandler _resourceHandler;
    private ISaveSystem _saveSystem;
    private IGoldHandler _goldHandler;
    private IResourceCatcher _resourceCatcher;
    
    [Inject]
    private void Construct(ISaveSystem saveSystem, IResourceHandler resourceHandler, IWeaponUpgrader weaponUpgrader, IGoldHandler goldHandler,IResourceCatcher resourceCatcher,  PlanetServicesProvider planetServicesProvider)
    {
        _weaponUpgrader = weaponUpgrader;
        _saveSystem = saveSystem;
        _goldHandler = goldHandler;
        _resourceHandler = resourceHandler;
        _baseUpgrader = planetServicesProvider.BaseUpgrader;
        _resourceCatcher = resourceCatcher;
        _rocketBuilder = planetServicesProvider.RocketBuilder;
        
        _weaponUpgrader.WeaponUpgraded += _saveSystem.SaveProgress;
        _goldHandler.AmountChanged += _saveSystem.SaveProgress;
        _resourceHandler.ResourcesCleared += _saveSystem.SaveProgress;
        _resourceHandler.ResourceAmountChanged += _saveSystem.SaveProgress;
        //_resourceCatcher.StoppedGatheringResources += _saveSystem.SaveProgress;
        _baseUpgrader.BaseUpgraded += _saveSystem.SaveProgress;
        _rocketBuilder.OnePartUpgraded += _saveSystem.SaveProgress;
        //_saveSystem.LoadProgress();
    }

    private void OnDisable()
    {
        _weaponUpgrader.WeaponUpgraded -= _saveSystem.SaveProgress;
        _goldHandler.AmountChanged -= _saveSystem.SaveProgress;
        _resourceHandler.ResourcesCleared -= _saveSystem.SaveProgress;
        _resourceCatcher.StoppedGatheringResources -= _saveSystem.SaveProgress;
        _baseUpgrader.BaseUpgraded -= _saveSystem.SaveProgress;
        _rocketBuilder.OnePartUpgraded -= _saveSystem.SaveProgress;
    }
}