using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(ParticleSpawner))]
public class BaseSellingSystem : MonoBehaviour, IInteractable
{
    private const int IronValue = 1;
    private const int CrystalValue = 2;
    private const int PlantValue = 4;
    private const int AlienArtifactValue = 6;
    
    [SerializeField] private Transform _baseStartSellingPoint;
    [SerializeField] private Transform _baseFinishSellingPoint;
    [SerializeField] private float _lerpDuration;
    [SerializeField] private SoundPlayer _resourceGatherSound;
    [SerializeField] private Iron _ironPrefab;
    [SerializeField] private Crystal _crystalPrefab;
    [SerializeField] private Plant _plantPrefab;
    [SerializeField] private AlienArtifact _alinArtifactPrefab;
    [SerializeField] private Transform _resourceToLerpSpawnPoint;
    
    private Iron _ironToLerp;
    private Crystal _crystalToLerp;
    private Plant _plantToLerp;
    private AlienArtifact _alienArtifactToLerp;

    private ParticleSpawner _glowingZoneParticleSystem;
    private Player _player;
    private Dictionary<Type, int> _resourcesToProcess;
    private bool _isProcessing;

    public event Action<int> IndicatedResourceValue;

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
        _player.ResourcesProvidedToBase += EnqueueResources;
        _glowingZoneParticleSystem = GetComponent<ParticleSpawner>();
        _player.ResourceAddedToBag += _glowingZoneParticleSystem.ActivateParticle;
    }

    private void Awake()
    {
        _ironToLerp = Instantiate(_ironPrefab, _resourceToLerpSpawnPoint.position, Quaternion.identity);
        _crystalToLerp = Instantiate(_crystalPrefab, _resourceToLerpSpawnPoint.position, Quaternion.identity);
        _plantToLerp = Instantiate(_plantPrefab, _resourceToLerpSpawnPoint.position, Quaternion.identity);
        _alienArtifactToLerp = Instantiate(_alinArtifactPrefab, _resourceToLerpSpawnPoint.position, Quaternion.identity);
    }

    private void OnDisable()
    {
        _player.ResourcesProvidedToBase -= EnqueueResources;
        _player.ResourceAddedToBag -= _glowingZoneParticleSystem.ActivateParticle;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<PlayerCollisionHandler>(out PlayerCollisionHandler player))
        {
            player.ProcessCollision(this);
        }
    }

    private void EnqueueResources(Dictionary<Type, int> resources)
    {
        _glowingZoneParticleSystem.DeactivateParticle();

        _resourcesToProcess = new Dictionary<Type, int>(resources);
        
        if (!_isProcessing)
        {
            StartCoroutine(ProcessResourceQueue());
        }
    }

    private IEnumerator ProcessResourceQueue()
    {
        _isProcessing = true;

        foreach (var resourcePair in _resourcesToProcess)
        {
            for (int i = 0; i < resourcePair.Value; i++)
            {
                _resourceGatherSound.PlaySound();
                IndicateResourceValue(resourcePair.Key, resourcePair.Value);
                yield return LerpResource(resourcePair.Key);
            }
        }
        
        _isProcessing = false;
    }

    private IEnumerator LerpResource(Type resourceType)
    {
        yield return StartCoroutine(TargetPositionLerper.LerpToTargetPosition(
            resourceType == typeof(Iron) ? _ironToLerp.transform :
            resourceType == typeof(Crystal) ? _crystalToLerp.transform :
            resourceType == typeof(Plant) ? _plantToLerp.transform :
            _alienArtifactToLerp.transform,
            _baseStartSellingPoint.position, _baseFinishSellingPoint.position, _lerpDuration));
    }

    private void IndicateResourceValue(Type resourceType, int count)
    {
        if (resourceType == typeof(Iron))
        {
            IndicatedResourceValue?.Invoke(count * IronValue);
        }
        else if (resourceType == typeof(Crystal))
        {
            IndicatedResourceValue?.Invoke(count * CrystalValue);
        }
        else if (resourceType == typeof(Plant))
        {
            IndicatedResourceValue?.Invoke(count * PlantValue);
        }
        else if (resourceType == typeof(AlienArtifact))
        {
            IndicatedResourceValue?.Invoke(count * AlienArtifactValue);
        }
    }
}

