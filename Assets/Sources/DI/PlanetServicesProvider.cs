using UnityEngine;
using Zenject;

public class PlanetServicesProvider : MonoInstaller
{
    [SerializeField] private Transform _planetPosition;
    [SerializeField] private UpgradeSystem _upgradeSystem;
    [SerializeField] private PlayerToBasePointerView _basePointerView;
    [SerializeField] private BaseUpgrader _baseUpgrader;

    public Transform PlanetPosition => _planetPosition;
    public UpgradeSystem UpgradeSystem => _upgradeSystem;
    public PlayerToBasePointerView BasePointerView => _basePointerView;
    public BaseUpgrader BaseUpgrader => _baseUpgrader;

}
