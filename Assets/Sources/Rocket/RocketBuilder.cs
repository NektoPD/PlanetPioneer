using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RocketBuilder : MonoBehaviour
{
    private const int MaxParts = 5;

    [SerializeField] private UpgradeSystem _upgradeSystem;

    private RocketPart[] _parts;
    private int _currentBuildParts = 0;
    private Collider _collider;
    private bool _isRocketFullyBuilt = false;

    public event Action OnePartUpgraded;
    public event Action RocketReady;

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
            Debug.LogWarning("Rocket is already fully built.");
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
                Debug.Log("Rocket is fully built.");
                RocketReady?.Invoke();
            }
        }
        else
        {
            Debug.LogWarning("No more parts available to upgrade.");
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
}
