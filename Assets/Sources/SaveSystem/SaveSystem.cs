using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

public class SaveSystem : ISaveSystem
{
    private const string WeaponLevelKey = "WeaponLevel";
    private const string BaseUpgradesLeftKey = "BaseLevel";
    private const string RocketLevelKey = "RocketLevel";
    private const string GoldAmountKey = "GoldAmount";
    private const string PlayerXPositionKey = "XPosition";
    private const string PlayerYPositionKey = "YPosition";
    private const string PlayerZPositionKey = "ZPosition";
    private const string PlayerTimerValue = "Timer";

    private BaseUpgrader _baseUpgrader;
    private RocketBuilder _rocketBuilder;
    private IWeaponUpgrader _weaponUpgrader;
    private IResourceHandler _catchedResourceHandler;
    private IGoldHandler _goldHandler;
    private Player _player;
    private ScoreSystem _scoreSystem;
    private string _filePath = Application.persistentDataPath + "/Save.json";

    [Inject]
    private void Construct(Player player, IResourceHandler resourceHandler, IWeaponUpgrader weaponUpgrader,
        IGoldHandler goldHandler,
        PlanetServicesProvider planetServicesProvider)
    {
        _player = player;
        _weaponUpgrader = weaponUpgrader;
        _catchedResourceHandler = resourceHandler;
        _goldHandler = goldHandler;
        _baseUpgrader = planetServicesProvider.BaseUpgrader;
        _rocketBuilder = planetServicesProvider.RocketBuilder;
        _scoreSystem = planetServicesProvider.ScoreSystem;
    }

    public void LoadProgress()
    {
        LoadPlayerPrefs();
        LoadResourcesFromJson();
    }

    public void SaveProgress()
    {
        SavePlayerPrefs();
        SaveResourcesToJson();
    }

    private void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt(WeaponLevelKey, _weaponUpgrader.CurrentLevel);
        PlayerPrefs.SetInt(BaseUpgradesLeftKey, _baseUpgrader.RemainingUpgrades);
        PlayerPrefs.SetInt(RocketLevelKey, _rocketBuilder.CurrentBuildParts);
        PlayerPrefs.SetInt(GoldAmountKey, _goldHandler.GoldAmount);
        PlayerPrefs.SetFloat(PlayerXPositionKey, _player.CurrentXPosition);
        PlayerPrefs.SetFloat(PlayerYPositionKey, _player.CurrentYPosition);
        PlayerPrefs.SetFloat(PlayerZPositionKey, _player.CurrentZPosition);
        PlayerPrefs.SetFloat(PlayerTimerValue, _scoreSystem.Timer);
        PlayerPrefs.Save();
    }

    private void SaveResourcesToJson()
    {
        var resourceDtos = GetSaveResourcesData();
        var resourceListDto = new ResourceListDto { Resources = resourceDtos };

        try
        {
            string json = JsonUtility.ToJson(resourceListDto, true);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save resources: {ex.Message}");
        }
    }

    private void LoadResourcesFromJson()
    {
        if (!File.Exists(_filePath))
            return;

        try
        {
            string json = File.ReadAllText(_filePath);
            var resourceListDto = JsonUtility.FromJson<ResourceListDto>(json);

            foreach (var resource in resourceListDto.Resources)
            {
                switch (resource.ResourceType)
                {
                    case ResourceType.Iron:
                        _catchedResourceHandler.SetResourceAmount(typeof(Iron), resource.Count);
                        break;
                    case ResourceType.Crystal:
                        _catchedResourceHandler.SetResourceAmount(typeof(Crystal), resource.Count);
                        break;
                    case ResourceType.Plant:
                        _catchedResourceHandler.SetResourceAmount(typeof(Plant), resource.Count);
                        break;
                    case ResourceType.AlienArtifact:
                        _catchedResourceHandler.SetResourceAmount(typeof(AlienArtifact), resource.Count);
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load resources: {ex.Message}");
        }
    }

    private void LoadPlayerPrefs()
    {
        LoadWeaponUpgrade();
        LoadBaseUpgrade();
        LoadRocketLevel();
        LoadGoldAmount();
        LoadPlayerPosition();
        LoadTimerValue();
    }

    private void LoadTimerValue()
    {
        if(PlayerPrefs.HasKey(PlayerTimerValue))
            _scoreSystem.SetTimer(PlayerPrefs.GetFloat(PlayerTimerValue));
    }

    private void LoadPlayerPosition()
    {
        if (PlayerPrefs.HasKey(PlayerXPositionKey) && PlayerPrefs.HasKey(PlayerYPositionKey) &&
            PlayerPrefs.HasKey(PlayerZPositionKey))
        {
            float x = PlayerPrefs.GetFloat(PlayerXPositionKey);
            float y = PlayerPrefs.GetFloat(PlayerYPositionKey);
            float z = PlayerPrefs.GetFloat(PlayerZPositionKey);
            _player.SetCurrentPosition(new Vector3(x, y, z));
        }
    }

    private void LoadWeaponUpgrade()
    {
        if (PlayerPrefs.HasKey(WeaponLevelKey))
        {
            _weaponUpgrader.SetCurrentLevel(PlayerPrefs.GetInt(WeaponLevelKey));
        }
    }

    private void LoadBaseUpgrade()
    {
        if (PlayerPrefs.HasKey(BaseUpgradesLeftKey))
        {
            _baseUpgrader.SetCurrentUpgrades(PlayerPrefs.GetInt(BaseUpgradesLeftKey));
        }
    }

    private void LoadRocketLevel()
    {
        if (PlayerPrefs.HasKey(RocketLevelKey))
        {
            _rocketBuilder.SetCurrenBuildParts(PlayerPrefs.GetInt(RocketLevelKey));
        }
    }

    private void LoadGoldAmount()
    {
        if (PlayerPrefs.HasKey(GoldAmountKey))
        {
            _goldHandler.SetGoldAmount(PlayerPrefs.GetInt(GoldAmountKey));
        }
    }

    private List<ResourceDto> GetSaveResourcesData()
    {
        return new List<ResourceDto>()
        {
            new() { ResourceType = ResourceType.Iron, Count = _catchedResourceHandler.CurrentIronAmount },
            new() { ResourceType = ResourceType.Crystal, Count = _catchedResourceHandler.CurrentCrystalAmount },
            new() { ResourceType = ResourceType.Plant, Count = _catchedResourceHandler.CurrentPlantAmount },
            new() { ResourceType = ResourceType.AlienArtifact, Count = _catchedResourceHandler.CurrentAlienArtifactAmount }
        };
    }
}

[Serializable]
public struct ResourceDto
{
    public ResourceType ResourceType;
    public int Count;
}

[Serializable]
public struct ResourceListDto
{
    public List<ResourceDto> Resources;
}

public enum ResourceType
{
    Iron,
    Crystal,
    Plant,
    AlienArtifact
}