using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSpawner))]
public class Gold : MonoBehaviour, IInteractable
{
    [SerializeField] private SoundController _obtainedSound;

    private ParticleSpawner _particleSpawner;
    private Transform _transform;
    private float _lerpDuration = 0.4f;

    public event Action<Gold> IsObtained;

    private void Awake()
    {
        _transform = transform;
        _particleSpawner = GetComponent<ParticleSpawner>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<PlayerCollisionHandler>(out PlayerCollisionHandler player))
        {
            StartCoroutine(ProcessCollision(player));
        }
    }

    private IEnumerator ProcessCollision(PlayerCollisionHandler player)
    {
        _obtainedSound.PlaySound();
        _particleSpawner.ActivateParticle();

        yield return StartCoroutine(TargetPositionLerper.LerpToTargetPosition(_transform, _transform.position, player.transform.position, _lerpDuration));

        player.ProcessCollision(this);
        IsObtained?.Invoke(this);
    }
}