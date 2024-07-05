using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private int _capacity;
    [SerializeField] private Transform _planetPosition;

    private readonly Queue<T> _queue = new Queue<T>();
    private readonly List<T> _activeObjects = new List<T>();

    protected void Initalize(T prefab)
    {
        for (int i = 0; i < _capacity; i++)
        {
            T spawnedObject = Instantiate(prefab, _container.transform.position, Quaternion.identity);
            spawnedObject.gameObject.SetActive(false);

            if (spawnedObject.TryGetComponent<PlayerPlanetRotationConstrain>(out PlayerPlanetRotationConstrain constraint))
                constraint.SetPlanetPosition(_planetPosition);

            _queue.Enqueue(spawnedObject);
        }
    }

    protected bool TryGetObject(out T @object, T prefab)
    {
        if (_queue.Count > 0)
        {
            @object = _queue.Dequeue();
            _activeObjects.Add(@object);

            return @object != null && @object.gameObject.activeSelf == false;
        }
        else
        {
            @object = Instantiate(prefab, _container.transform);
            @object.gameObject.SetActive(false);
            _activeObjects.Add(@object);

            return @object != null;
        }
    }

    protected void PutObject(T @object)
    {
        _queue.Enqueue(@object);
        @object.gameObject.SetActive(false);
        _activeObjects.Remove(@object);
    }
}
