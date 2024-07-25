using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class GoldSpawner : ObjectPool<Gold>
{
    [SerializeField] private Gold _prefab;
    [SerializeField] private int _spawnCount;
    [SerializeField] private Transform _startSpawnPosition;
    [SerializeField] private Transform _endSpawnPosition;
    [SerializeField] private float _lerpSpeed;
    [SerializeField] private BaseSellingSystem _sellingSystem;

    private MeshRenderer _planeMesh;

    private bool _isSpawning;

    private void Awake()
    {
        _planeMesh = GetComponent<MeshRenderer>();
        Initalize(_prefab);
    }

    private void OnEnable()
    {
        _sellingSystem.IndicatedResource += StartSpawning;
    }

    private void OnDisable()
    {
        _sellingSystem.IndicatedResource -= StartSpawning;
    }

    private IEnumerator SpawnGold(int spawnCount)
    {
        _isSpawning = true;

        for (int i = 0; i <= spawnCount; i++)
        {
            if (TryGetObject(out Gold gold, _prefab))
            {
                gold.transform.position = _startSpawnPosition.position;
                gold.IsObtained += ReturnToPull;
                yield return StartCoroutine(TargetPositionLerper.LerpToTargetPosition(gold.transform, _startSpawnPosition.position, _endSpawnPosition.position, _lerpSpeed));
                Vector3 randomPosition = GetRandomPointInPlane();
                gold.transform.position = randomPosition;
            }
        }

        _isSpawning = false;
    }


    private void StartSpawning(int spawnCount)
    {
        if (!_isSpawning)
            StartCoroutine(SpawnGold(spawnCount));
    }

    private Vector3 GetRandomPointInPlane()
    {
        Bounds bounds = _planeMesh.bounds;
        Vector3 randomPoint = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.max.y,
            Random.Range(bounds.min.z, bounds.max.z)
        );

        return randomPoint;
    }

    private void ReturnToPull(Gold gold)
    {
        gold.IsObtained -= ReturnToPull;
        PutObject(gold);
    }

    public Gold GetGold()
    {
        if (TryGetObject(out Gold gold, _prefab))
        {
            gold.IsObtained += PutObject;
            return gold;
        }

        return null;
    }
}
