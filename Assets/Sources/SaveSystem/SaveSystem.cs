using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SaveSystem : ISaveSystem
{
    private const string SaveKey = "SaveData";

    private BaseUpgrader _baseUpgrader;
    private RocketBuilder _rocketBuilder;
    private IWeaponUpgrader _weaponUpgrader;
    private IResourceHandler _catchedResourceHandler;
    private IGoldHandler _goldHandler;
    private Player _player;
    private ScoreSystem _scoreSystem;

    public enum ResourceType
    {
        Iron,
        Crystal,
        Plant,
        AlienArtifact
    }

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
        if (!PlayerPrefs.HasKey(SaveKey))
            return;

        try
        {
            string json = PlayerPrefs.GetString(SaveKey);
            var saveData = JsonUtility.FromJson<SaveData>(json);

            _goldHandler.SetGoldAmount(saveData.GoldAmount);
            _player.SetCurrentPosition(new Vector3(saveData.PlayerXPosition, saveData.PlayerYPosition,
                saveData.PlayerZPosition));
            _scoreSystem.SetTimer(saveData.Timer);

            foreach (var resource in saveData.Resources)
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

            _baseUpgrader.SetCurrentUpgrades(saveData.BaseUpgradesLeft);
            _rocketBuilder.SetCurrenBuildParts(saveData.RocketLevel);
            _weaponUpgrader.SetCurrentLevel(saveData.WeaponLevel);
        }
        catch (Exception ex)
        {
            throw new ArgumentException(nameof(ex.Message));
        }
    }

    public void SaveProgress()
    {
        var saveData = new SaveData
        {
            WeaponLevel = _weaponUpgrader.CurrentLevel,
            BaseUpgradesLeft = _baseUpgrader.RemainingUpgrades,
            RocketLevel = _rocketBuilder.CurrentBuildParts,
            GoldAmount = _goldHandler.GoldAmount,
            PlayerXPosition = _player.CurrentXPosition,
            PlayerYPosition = _player.CurrentYPosition,
            PlayerZPosition = _player.CurrentZPosition,
            Timer = _scoreSystem.Timer,
            Resources = GetSaveResourcesData()
        };

        try
        {
            string json = JsonUtility.ToJson(saveData, true);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }
        catch (Exception ex)
        {
            throw new ArgumentException(nameof(ex.Message));
        }
    }

    public void ResetData()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            PlayerPrefs.DeleteKey(SaveKey);
        }
    }

    private List<ResourceDto> GetSaveResourcesData()
    {
        return new List<ResourceDto>
        {
            new ResourceDto { ResourceType = ResourceType.Iron, Count = _catchedResourceHandler.CurrentIronAmount },
            new ResourceDto
                { ResourceType = ResourceType.Crystal, Count = _catchedResourceHandler.CurrentCrystalAmount },
            new ResourceDto { ResourceType = ResourceType.Plant, Count = _catchedResourceHandler.CurrentPlantAmount },
            new ResourceDto
            {
                ResourceType = ResourceType.AlienArtifact, Count = _catchedResourceHandler.CurrentAlienArtifactAmount
            }
        };
    }

    [Serializable]
    public struct ResourceDto
    {
        public ResourceType ResourceType;
        public int Count;
    }

    [Serializable]
    public class SaveData
    {
        public int WeaponLevel;
        public int BaseUpgradesLeft;
        public int RocketLevel;
        public int GoldAmount;
        public float PlayerXPosition;
        public float PlayerYPosition;
        public float PlayerZPosition;
        public float Timer;
        public List<ResourceDto> Resources = new List<ResourceDto>();
    }
}