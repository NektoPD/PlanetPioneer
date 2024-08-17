using UnityEngine;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Player _prefab;
    [SerializeField] private PlanetServicesProvider _planetServicesProvider;
    [SerializeField] private UIServicesProvider _UIServicesProvider;

    public override void InstallBindings()
    {
        BindSaveSystem();
        BindUIServices();
        BindPlanetServices();
        BindPlayer();
    }

    private void BindSaveSystem()
    {
        Container.Bind<ISaveSystem>().To<SaveSystem>().FromNew().AsSingle();
    }

    private void BindUIServices()
    {
        Container.Bind<UIServicesProvider>().FromInstance(_UIServicesProvider).AsSingle();
    }

    private void BindPlanetServices()
    {
        Container.Bind<PlanetServicesProvider>().FromInstance(_planetServicesProvider).AsSingle();
    }

    private void BindPlayer()
    {
        Player player = Container.InstantiatePrefabForComponent<Player>(_prefab, _startPoint.position, Quaternion.identity, null);

        Container.Bind<Player>().FromInstance(player).AsSingle();
        Container.Bind<IWeaponUpgrader>().FromInstance(player.GetComponentInChildren<WeaponUpgrader>()).AsSingle();
        Container.Bind<IResourceHandler>().FromInstance(player.GetComponentInChildren<CatchedResourceHandler>())
            .AsSingle();
        Container.Bind<IGoldHandler>().FromInstance(player.GetComponentInChildren<PlayerGoldHandler>()).AsSingle();
        Container.Bind<IResourceCatcher>().FromInstance(player.GetComponentInChildren<ResourceCatcher>()).AsSingle();
    }
}
