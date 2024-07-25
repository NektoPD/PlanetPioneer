using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSellingSystem : MonoBehaviour, IInteractable
{
    [SerializeField] private ResourceSpawner _ironResourceSpawner;
    [SerializeField] private ResourceSpawner _crystalResourceSpawner;
    [SerializeField] private ResourceSpawner _plantResourceSpawner;
    [SerializeField] private ResourceSpawner _alienArtifactResourceSpawner;
    [SerializeField] private Transform _baseStartSellingPoint;
    [SerializeField] private Transform _baseFinishSellingPoint;
    [SerializeField] private float _lerpDuration;
    [SerializeField] private Player _player; // Zenject?

    private readonly Queue<Resource> _resourceQueue = new Queue<Resource>();
    private bool _isProcessing = false;
    private int _ironValue = 1;
    private int _crystalValue = 2;
    private int _plantValue = 4;
    private int _alienArtifactValue = 6;

    public event Action<int> IndicatedResource;
    public event Action<Resource> GotResource;

    private void OnEnable()
    {
        _player.ResourcesProvidedToBase += EnqueueResources;
    }

    private void OnDisable()
    {
        _player.ResourcesProvidedToBase -= EnqueueResources;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<PlayerCollisionHandler>(out PlayerCollisionHandler player))
        {
            player.ProcessCollision(this); //Заменить на событие?
        }
    }

    private void EnqueueResources(Dictionary<Type, List<Resource>> resources)
    {
        foreach (var resourcePair in resources)
        {
            foreach (var resource in resourcePair.Value)
            {
                _resourceQueue.Enqueue(resource);
            }
        }

        if (!_isProcessing)
        {
            StartCoroutine(ProcessResourceQueue());
        }
    }

    private IEnumerator ProcessResourceQueue()
    {
        _isProcessing = true;

        while (_resourceQueue.Count > 0)
        {
            Resource resource = _resourceQueue.Dequeue();

            yield return StartCoroutine(TargetPositionLerper.LerpToTargetPosition(resource.transform, _baseStartSellingPoint.position, _baseFinishSellingPoint.position, _lerpDuration));
            Type resourceType = resource.GetType();

            switch (resourceType)
            {
                case Type t when t == typeof(Iron):
                    IndicatedResource?.Invoke(_ironValue);
                    _ironResourceSpawner.ReturnResourceToPull(resource);
                    break;
                case Type t when t == typeof(Crystal):
                    IndicatedResource?.Invoke(_crystalValue);
                    _crystalResourceSpawner.ReturnResourceToPull(resource);
                    break;
                case Type t when t == typeof(Plant):
                    IndicatedResource?.Invoke(_plantValue);
                    _plantResourceSpawner.ReturnResourceToPull(resource);
                    break;
                case Type t when t == typeof(AlienArtifact):
                    IndicatedResource?.Invoke(_alienArtifactValue);
                    _alienArtifactResourceSpawner.ReturnResourceToPull(resource);
                    break;
                default:
                    Debug.LogWarning("Unhandled resource type: " + resourceType.Name);
                    break;
            }

            GotResource?.Invoke(resource);
        }

        _isProcessing = false;
    }
}

