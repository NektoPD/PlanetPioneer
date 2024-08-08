using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Zenject;

public class SaveSystem : ISaveSystem
{
    [SerializeField] private BaseUpgrader _baseUpgrader;
    [SerializeField] private RocketBuilder _rocketBuilder;
    [SerializeField] private ResourceSpawner _ironSpawner;

    private WeaponUpgrader _weaponUpgrader;
    private CatchedResourceHandler _catchedResourceHandler;
    private Player _player;
    private string _filePath;

    private const string WeaponLevelKey = "WeaponLevel";
    private const string BaseUpgradesLeftKey = "BaseLevel";
    private const string RocketLevelKey = "RocketLevel";
    private const string GoldAmountKey = "GoldAmount";
    private const string PlayerXPositionKey = "XPosition";
    private const string PlayerYPositionKey = "YPosition";
    private const string PlayerZPositionKey = "ZPosition";

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
        _weaponUpgrader = player.GetComponentInChildren<WeaponUpgrader>();
        _catchedResourceHandler = player.GetComponentInChildren<CatchedResourceHandler>();
    }

    private void Awake()
    {
        _filePath = Application.persistentDataPath + "/Save.json";
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
        PlayerPrefs.SetInt(GoldAmountKey, _player.CurrentGoldAmount);
        PlayerPrefs.SetFloat(PlayerXPositionKey, _player.CurrentXPosition);
        PlayerPrefs.SetFloat(PlayerYPositionKey, _player.CurrentYPosition);
        PlayerPrefs.SetFloat(PlayerZPositionKey, _player.CurrentZPosition);
        PlayerPrefs.Save();
    }

    private void SaveResourcesToJson()
    {
        List<ResourceDto> resourceDtos = GetSaveResourcesData();
        try
        {
            using (var writer = new StreamWriter(_filePath))
            {
                foreach (var resource in resourceDtos)
                {
                    string json = JsonUtility.ToJson(resource);
                    writer.WriteLine(json);
                }
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(nameof(ex));
        }
    }

    private void LoadResourcesFromJson()
    {
        if (!File.Exists(_filePath))
            return;

        var resources = new List<ResourceDto>();
        try
        {
            string[] lines = File.ReadAllLines(_filePath);
            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    ResourceDto resource = JsonUtility.FromJson<ResourceDto>(line);
                    resources.Add(resource);
                }
            }

            _catchedResourceHandler.SetResourceAmount(resources.SelectMany(r => r.Resources).ToList());
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(nameof(ex));
        }
    }
    
    private void LoadPlayerPrefs()
    {
        LoadWeaponUpgrade();
        LoadBaseUpgrade();
        LoadRocketLevel();
        LoadGoldAmount();
        LoadPlayerPosition();
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
            _player.ProcessGoldIncreaseAmount(PlayerPrefs.GetInt(GoldAmountKey));
        }
    }

    private List<ResourceDto> GetSaveResourcesData()
    {
        return new List<ResourceDto>()
        {
            CreateResourceDataToSave(_catchedResourceHandler.CurrentIronAmount),
            CreateResourceDataToSave(_catchedResourceHandler.CurrentCrystalAmount),
            CreateResourceDataToSave(_catchedResourceHandler.CurrentPlantAmount),
            CreateResourceDataToSave(_catchedResourceHandler.CurrentAlienArtifactAmount)
        };
    }

    private ResourceDto CreateResourceDataToSave(List<Resource> resources)
    {
        return new ResourceDto { Resources = resources };
    }
}

[Serializable]
public class ResourceDto
{
    public List<Resource> Resources;
}