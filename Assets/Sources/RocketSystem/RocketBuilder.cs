using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RocketBuilder : MonoBehaviour
{
    private const string RocketIsFullyBuiltMessage = "Rocket is fully built.";
    private const string NoRocketUpgradesAvailableMessage = "No more parts available to upgrade.";
    private const int MaxParts = 5;

    [SerializeField] private UIPopUpWindowShower _popUpWindowShower;
    [SerializeField] private UpgradeSystem _upgradeSystem;
    [SerializeField] private SoundController _rocketBuidSound;

    private RocketPart[] _parts;
    private int _currentBuildParts = 0;
    private Collider _collider;
    private bool _isRocketFullyBuilt = false;

    public event Action OnePartUpgraded;
    public event Action RocketReady;

    public int CurrentBuildParts => _currentBuildParts;

    private void Awake()
    {
        _parts = GetComponentsInChildren<RocketPart>();
        _collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        _upgradeSystem.RocketUpgraded += BuildOneRocketPart;
        _upgradeSystem.RocketUpgradesAvailable += EnableRocketCollider;
    }

    private void OnDisable()
    {
        _upgradeSystem.RocketUpgraded -= BuildOneRocketPart;
        _upgradeSystem.RocketUpgradesAvailable -= EnableRocketCollider;
    }

    private void Start()
    {
        foreach (RocketPart part in _parts)
        {
            part.gameObject.SetActive(false);
        }

        Collider collider = GetComponent<Collider>();
        collider.enabled = false;

        DisableRocketCollider();
    }

    public void BuildOneRocketPart()
    {
        if (_isRocketFullyBuilt)
        {
            _popUpWindowShower.AddMessageToQueue(RocketIsFullyBuiltMessage);
            return;
        }

        RocketPart currentPart = _parts.FirstOrDefault(part => !part.gameObject.activeSelf);

        if (currentPart != null)
        {
            currentPart.gameObject.SetActive(true);
            _currentBuildParts++;
            OnePartUpgraded?.Invoke();

            if (_currentBuildParts == MaxParts)
            {
                _isRocketFullyBuilt = true;
                _popUpWindowShower.AddMessageToQueue(RocketIsFullyBuiltMessage);
                _rocketBuidSound.PlaySound();
                RocketReady?.Invoke();
            }
        }
        else
        {
            _popUpWindowShower.AddMessageToQueue(NoRocketUpgradesAvailableMessage);
        }
    }

    private void EnableRocketCollider()
    {
        _collider.enabled = true;
    }

    private void DisableRocketCollider()
    {
        _collider.enabled = false;
    }

    public void SetCurrenBuildParts(int currentBuildParts)
    {
        if(currentBuildParts > MaxParts || currentBuildParts < 0)
            throw new ArgumentOutOfRangeException(nameof(currentBuildParts));

        _currentBuildParts = currentBuildParts;

        for(int i = 0; i < _currentBuildParts; i++)
        {
            OnePartUpgraded?.Invoke();
        }

        if (_currentBuildParts == MaxParts)
        {
            _isRocketFullyBuilt = true;
            RocketReady?.Invoke();
        }
    }
}
