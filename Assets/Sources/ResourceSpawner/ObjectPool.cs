using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private int _capacity;
    [SerializeField] private Transform _planetPosition;

    private readonly Queue<T> _queue = new Queue<T>();
    private readonly List<T> _activeObjects = new List<T>();

    public event Action ObjectReturnedToPool;
    
    public Transform PlanetPosition => _planetPosition;
    public int Capacity => _capacity;
    public IReadOnlyCollection<T> ActiveObjects => _activeObjects;

    protected void Initalize(T prefab)
    {
        for (int i = 0; i < _capacity; i++)
        {
            T spawnedObject = Instantiate(prefab, _container.transform.position, Quaternion.identity);
            spawnedObject.gameObject.SetActive(false);

            if (spawnedObject.TryGetComponent<PlanetRotationConstrain>(out PlanetRotationConstrain rotationConstaint))
                rotationConstaint.SetPlanetPosition(_planetPosition);

            _queue.Enqueue(spawnedObject);
        }
    }

    protected bool TryGetObject(out T @object, T prefab)
    {
        if (_queue.Count > 0)
        {
            @object = _queue.Dequeue();
            _activeObjects.Add(@object);
            @object.gameObject.SetActive(true);
            return true;
        }
        else
        {
            @object = Instantiate(prefab);
            @object.GetComponent<PlanetRotationConstrain>().SetPlanetPosition(_planetPosition);
            return true;
        }
    }

    protected void PutObject(T @object)
    {
        if (@object == null)
            throw new ArgumentNullException(nameof(@object));

        @object.gameObject.SetActive(false);
        @object.transform.position = _container.transform.position;
        _activeObjects.Remove(@object);
        _queue.Enqueue(@object);

        ObjectReturnedToPool?.Invoke();
    }
}
