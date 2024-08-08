using UnityEngine;
using Zenject;

public class LocationInstaller : MonoInstaller
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Player _prefab;
    [SerializeField] private PlanetServicesProvider _planetServicesProvider;
    [SerializeField] private UIServicesProvider _UIServicesProvider;

    public override void InstallBindings()
    {
        BindUIServices();
        BindPlanetServices();
        BindPlayer();
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
    }
}
