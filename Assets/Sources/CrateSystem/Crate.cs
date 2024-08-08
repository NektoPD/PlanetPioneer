using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(CrateAnimator))]
public class Crate : MonoBehaviour
{
    private GoldSpawner _goldSpawner;
    private CrateAnimator _animator;
    private CrateSpawnArea _spawnArea;
    private Transform[] _spawnAreaPoints;

    public event Action<Crate> Exploded;

    private void Awake()
    {
        _animator = GetComponent<CrateAnimator>();
        _spawnArea = GetComponentInChildren<CrateSpawnArea>();
        _spawnAreaPoints = _spawnArea.GetComponentsInChildren<Transform>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            SpawnGold();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            Exploded?.Invoke(this);
        }
    }

    private void SpawnGold()
    {
        _animator.ActivateOpenTrigger();

        for (int i = 0; i < _spawnAreaPoints.Length; i++)
        {
            Gold goldToSpawn = _goldSpawner.GetGold();
            goldToSpawn.gameObject.SetActive(true);
            goldToSpawn.transform.position = _spawnAreaPoints[i].position;
        }
    }

    public void SetGoldSpawner(GoldSpawner goldSpawner)
    {
        if (goldSpawner == null)
            throw new ArgumentNullException(nameof(goldSpawner));

        _goldSpawner = goldSpawner;
    }
}
