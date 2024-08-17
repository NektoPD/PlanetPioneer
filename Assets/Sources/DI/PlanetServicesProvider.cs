using UnityEngine;
using Zenject;

public class PlanetServicesProvider : MonoInstaller
{
    [SerializeField] private Transform _planetPosition;
    [SerializeField] private UpgradeSystem _upgradeSystem;
    [SerializeField] private PlayerToBasePointerView _basePointerView;
    [SerializeField] private BaseUpgrader _baseUpgrader;
    [SerializeField] private RocketBuilder _rocketBuilder;
    [SerializeField] private BaseSellingSystem _baseSellingSystem;

    public Transform PlanetPosition => _planetPosition;
    public UpgradeSystem UpgradeSystem => _upgradeSystem;
    public BaseUpgrader BaseUpgrader => _baseUpgrader;
    public RocketBuilder RocketBuilder => _rocketBuilder;
    public BaseSellingSystem BaseSellingSystem => _baseSellingSystem;
}
