using System.Linq.Expressions;
using UnityEngine;

public class ResourceSpawner : ObjectPool<Resource>
{
    [SerializeField] private Resource _prefab;
    [SerializeField] private int _spawnCount = 10;

    private Collider _collider;
    private Vector3 _spawnAreaMin;
    private Vector3 _spawnAreaMax;

    private void Awake()
    {
        _collider = GetComponent<Collider>();

        _spawnAreaMin = _collider.bounds.min;
        _spawnAreaMax = _collider.bounds.max;

        Initalize(_prefab);
    }

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            Resource resource = null;

            if (TryGetObject(out resource, _prefab))
            {
                float randomXPosition = Random.Range(_spawnAreaMin.x, _spawnAreaMax.x);
                float randomYPosition = Random.Range(_spawnAreaMin.y, _spawnAreaMax.y);

                Vector3 spawnPosition = new Vector3(randomXPosition, randomYPosition, transform.position.z);

               resource.transform.position = spawnPosition;
               resource.gameObject.SetActive(true);
            }
            else
            {
                return;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_collider == null)
            _collider = GetComponent<Collider>();

        if (_collider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_collider.bounds.center, _collider.bounds.size);
        }
    }
}
