using UnityEngine;

[RequireComponent(typeof(ParticleSpawner))]
public class CrateSpawner : ObjectPool<Crate>
{
    [SerializeField] private GoldSpawner _goldSpawner;
    [SerializeField] private Crate _prefab;
    [SerializeField] private Transform _spawnPointPosition;
    [SerializeField] private BaseUpgrader _baseUpgrader;
    [SerializeField] private SoundPlayer _crateSpawnSound;
    [SerializeField] private VideoAd _videoAdShower;

    private ParticleSpawner _particleSpawner;

    private void Awake()
    {
        Initalize(_prefab);

        _particleSpawner = GetComponent<ParticleSpawner>();
    }

    private void OnEnable()
    {
        _baseUpgrader.BaseUpgraded += SpawnCrate;
        _videoAdShower.RewardedAdWatched += SpawnCrate;
    }

    private void OnDisable()
    {
        _baseUpgrader.BaseUpgraded -= SpawnCrate;
        _videoAdShower.RewardedAdWatched -= SpawnCrate;
    }
    
    public void SpawnCrate()
    {
        if(TryGetObject(out Crate crate, _prefab))
        {
            crate.transform.position = _spawnPointPosition.position;
            crate.transform.rotation = Quaternion.Euler(transform.position.x, 180f, transform.position.z);
            crate.Exploded += ReturnToPool;
            crate.SetGoldSpawner(_goldSpawner);
            _particleSpawner.ActivateParticle();
            _crateSpawnSound.PlaySound();
        }
    }

    private void ReturnToPool(Crate crate)
    {
        crate.Exploded -= ReturnToPool;
        PutObject(crate);
    }
}
