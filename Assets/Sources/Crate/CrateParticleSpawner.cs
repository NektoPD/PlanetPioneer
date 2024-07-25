using UnityEngine;

public class CrateParticleSpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosionPrefab;

    public void SpawnExplosion()
    {
        var explosionVFX = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
    }
}
