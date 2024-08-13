using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;

public class ResourceCatcher : MonoBehaviour,IResourceCatcher
{
    private const float UpgradedLerpDuration = 1f;
    private const float UpgradedRadius = 1.2f;

    [SerializeField] private float _radius;
    [SerializeField] private float _distance = 5;
    [SerializeField] private LayerMask _resourceMask;
    [SerializeField] private Transform _gunPosition;
    [SerializeField] private float _lerpDuration;
    [SerializeField] private Vector3 _targetScale;
    [SerializeField] private SoundPlayer _catchedResourceSound;
    [SerializeField] private SoundPlayer _errorSound;

    private IShooter _shooter;
    private ICapacityHandler _capacityHandler;
    private IWeaponLevelChecker _weaponLevelChecker;
    private IPlayerUpgrader _playerUpgrader;
    private UISliderShower _sliderShower;
    private UIPopUpWindowShower _popUpWindowShower;
    private Coroutine _gatheringCoroutine;
    private Transform _transform;

    public event Action<Resource> CatchedResource;
    public event Action StartedGatheringResources;
    public event Action StoppedGatheringResources;

    [Inject]
    private void Construct(UIServicesProvider UIServices)
    {
        _sliderShower = UIServices.UISlider;
        _popUpWindowShower = UIServices.PopUpWindow;
    }

    private void Awake()
    {
        _transform = transform;
    }

    private void OnDisable()
    {
        _shooter.ShootButtonPressed -= Shoot;
        _playerUpgrader.UpgradedGatherSpeed -= UpgradeResourceGatherSpeed;
        _playerUpgrader.UpgradedGatherRadius -= UpgradeResourceGatherRadius;
    }

    public void SetShooter(IShooter shooter)
    {
        if (shooter == null)
            throw new ArgumentNullException(nameof(shooter));

        _shooter = shooter;
        _shooter.ShootButtonPressed += Shoot;
    }

    public void SetWeaponLevelChecker(IWeaponLevelChecker weaponLevelChecker)
    {
        if (weaponLevelChecker == null)
            throw new ArgumentNullException(nameof(weaponLevelChecker));

        _weaponLevelChecker = weaponLevelChecker;
    }

    public void SetCapacityChecker(ICapacityHandler handler)
    {
        if (handler == null)
            throw new ArgumentNullException(nameof(handler));

        _capacityHandler = handler;
    }
    
    public void SetPlayerUpgrader(IPlayerUpgrader playerUpgrader)
    {
        if (playerUpgrader == null)
            throw new ArgumentNullException(nameof(playerUpgrader));

        _playerUpgrader = playerUpgrader;
        _playerUpgrader.UpgradedGatherSpeed += UpgradeResourceGatherSpeed;
        _playerUpgrader.UpgradedGatherRadius += UpgradeResourceGatherRadius;
    }

    private void Shoot()
    {
        if (_shooter == null)
            return;

        Vector3 shootDirection = _transform.position + _transform.forward * _distance;
        RaycastHit[] hits = Physics.SphereCastAll(_transform.position, _radius, shootDirection, _distance);

        var resources = hits.Select(hit => hit.collider.GetComponent<Resource>()).Where(resource => resource != null);

        foreach (Resource resource in resources)
        {
            Type resourceType = resource.GetType();

            if (_weaponLevelChecker.IsWeaponLevelSufficient(resource) == false)
            {
                _popUpWindowShower.AddMessageToQueue($"Weapon level is not enough to collect {resourceType.Name}");
                _errorSound.PlaySound();
                return;
            }

            if (_capacityHandler.IsMaxCapacityReached(resourceType))
            {
                _popUpWindowShower.AddMessageToQueue($"Cannot catch more {resourceType.Name}. Maximum capacity reached.");
                _errorSound.PlaySound();
                return;
            }

            if (resource.IsAbsorbed)
            {
                return;
            }

            if (_gatheringCoroutine != null)
            {
                StopCoroutine(_gatheringCoroutine);
            }

            _gatheringCoroutine = StartCoroutine(GatherResource(resource));
            resource.SetAbsorbedToTrue();
            CatchedResource?.Invoke(resource);
        }
    }

    private IEnumerator GatherResource(Resource resource)
    {
        StartedGatheringResources?.Invoke();

        _sliderShower.ActivateSlider(_lerpDuration);
        yield return StartCoroutine(TargetPositionLerper.LerpToTargetPosition(resource.transform, resource.transform.position, 
            _gunPosition.position, resource.transform.localScale, _targetScale,_lerpDuration));

        _catchedResourceSound.PlaySound();
        StoppedGatheringResources?.Invoke();
    }
    
    private void UpgradeResourceGatherSpeed()
    {
        _lerpDuration = UpgradedLerpDuration;
    }

    private void UpgradeResourceGatherRadius()
    {
        _radius = UpgradedRadius;
    }
}
