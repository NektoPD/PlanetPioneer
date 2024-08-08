using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particlePrefab;
    [SerializeField] private Transform _particleSpawnArea;

    private ParticleSystem _currentParticle;

    private void Awake()
    {
        _currentParticle = Instantiate(_particlePrefab, _particleSpawnArea.position, Quaternion.identity);
        DeactivateParticle();
    }

    public void ActivateParticle()
    {
        _currentParticle.transform.position = _particleSpawnArea.position;
        _currentParticle.Play();
    }

    public void DeactivateParticle()
    {
        _currentParticle.Stop();
    }
}
