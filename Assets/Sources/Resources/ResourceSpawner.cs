using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : ObjectPool<Resource>
{
    private const string NoAvailableSpawnPointsErrorMessage = "No spawn points";

    [SerializeField] private Resource _prefab;
    [SerializeField] private int _spawnCount = 10;
    [SerializeField] private BaseSellingSystem _sellingSystem;

    private readonly List<SpawnArea> _spawnPoints = new List<SpawnArea>();

    private void Awake()
    {
        _spawnPoints.AddRange(GetComponentsInChildren<SpawnArea>());

        Initalize(_prefab);
    }

    private void Start()
    {
        foreach (SpawnArea spawnArea in _spawnPoints)
        {
            SpawnInArea(spawnArea);
        }
    }

    private void OnEnable()
    {
        _sellingSystem.GotResource += ReturnResourceToPull;
        ObjectReturnedToPool += Spawn;

    }

    private void OnDisable()
    {
        _sellingSystem.GotResource -= ReturnResourceToPull;
        ObjectReturnedToPool -= Spawn;
    }

    private void Spawn()
    {
        if (_spawnPoints.Count == 0)
            throw new InvalidOperationException(NoAvailableSpawnPointsErrorMessage);

        List<SpawnArea> availableSpawnPoints = new List<SpawnArea>(_spawnPoints);

        for (int i = 0; i < _spawnCount; i++)
        {
            if (availableSpawnPoints.Count == 0)
                break;

            int randomIndex = UnityEngine.Random.Range(0, availableSpawnPoints.Count);
            SpawnArea selectedSpawnArea = availableSpawnPoints[randomIndex];
            availableSpawnPoints.RemoveAt(randomIndex);

            SpawnInArea(selectedSpawnArea);
        }
    }

    private void SpawnInArea(SpawnArea spawnArea)
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            if (TryGetObject(out Resource resource, _prefab))
            {
                spawnArea.SpawnResource(resource, PlanetPosition);
            }
        }
    }

    public void ReturnResourceToPull(Resource resource)
    {
        if(resource == null)
            throw new ArgumentNullException(nameof(resource));

        PutObject(resource);
        resource.SetInitScale();
        resource.SetAbsorbedToFalse();
    }
}
