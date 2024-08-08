using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;

public class ResourceCatcher : MonoBehaviour
{
    private const float UpgradedLerpDuration = 1f;
    private const float UpgradedRadius = 1.2f;

    [SerializeField] private float _radius;
    [SerializeField] private float _distance = 5;
    [SerializeField] private LayerMask _resourceMask;
    [SerializeField] private Transform _gunPosition;
    [SerializeField] private float _lerpDuration;
    [SerializeField] private Vector3 _targetScale;
    [SerializeField] private SoundController _catchedResourceSound;
    [SerializeField] private SoundController _errorSound;

    private UISliderShower _sliderShower;
    private UIPopUpWindowShower _popUpWindowShower;
    private Weapon _weapon;
    private ICapacityChecker _capacityChecker;
    private Coroutine _gatheringCoroutine;
    private PlayerUpgrader _playerUpgrader;

    public event Action<Resource> CatchedResource;
    public event Action StartedGatheringResources;
    public event Action StoppedGatheringResources;

    [Inject]
    private void Construct(UIServicesProvider UIServices)
    {
        _sliderShower = UIServices.UISlider;
        _popUpWindowShower = UIServices.PopUpWindow;
    }

    public IEnumerator LerpToGunPosition(Transform target)
    {
        if (_gunPosition == null)
            yield break;

        yield return StartCoroutine(TargetPositionLerper.LerpToTargetPosition(target, target.position, _gunPosition.position, target.transform.localScale, _targetScale, _lerpDuration));
    }

    public void SetWeapon(Weapon weapon)
    {
        if (weapon == null)
            throw new ArgumentNullException();

        _weapon = weapon;
        _weapon.ShootButtonPressed += Shoot;
    }

    public void SetCapacityChecker(ICapacityChecker checker)
    {
        if (checker == null)
            throw new ArgumentNullException(nameof(checker));

        _capacityChecker = checker;
    }

    private void Shoot()
    {
        if (_weapon == null)
            return;

        Vector3 shootOrigin = transform.position + transform.forward * _distance;
        RaycastHit[] hits = Physics.SphereCastAll(shootOrigin, _radius, transform.forward, _distance);

        var resources = hits.Select(hit => hit.collider.GetComponent<Resource>()).Where(resource => resource != null);

        foreach (Resource resource in resources)
        {
            Type resourceType = resource.GetType();

            if (_weapon.CanCollectResource(resource) == false)
            {
                _popUpWindowShower.AddMessageToQueue($"Weapon level is not enough to collect {resourceType.Name}");
                _errorSound.PlaySound();
                return;
            }

            if (_capacityChecker.IsMaxCapacityReached(resourceType))
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
        yield return StartCoroutine(LerpToGunPosition(resource.transform));

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

    public void SetPlayerUpgrader(PlayerUpgrader upgrader)
    {
        if (upgrader == null)
            throw new ArgumentNullException(nameof(upgrader));

        _playerUpgrader = upgrader;
        _playerUpgrader.UpgradedGatherSpeed += UpgradeResourceGatherSpeed;
        _playerUpgrader.UpgradedGatherRadius += UpgradeResourceGatherRadius;
    }

    private void OnDisable()
    {
        _weapon.ShootButtonPressed -= Shoot;
        _playerUpgrader.UpgradedGatherSpeed -= UpgradeResourceGatherSpeed;
        _playerUpgrader.UpgradedGatherRadius -= UpgradeResourceGatherRadius;
    }
}