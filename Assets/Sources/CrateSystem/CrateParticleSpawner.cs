using UnityEngine;

public class CrateParticleSpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem _appearancePrefab;

    public void SpawnParticles(Vector3 spawnPosition)
    {
        var explosionVFX = Instantiate(_appearancePrefab, spawnPosition, Quaternion.identity);
    }
}
