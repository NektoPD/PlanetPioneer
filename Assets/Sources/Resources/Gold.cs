using System;
using UnityEngine;

public class Gold : MonoBehaviour, IInteractable
{
    public event Action<Gold> IsObtained;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<PlayerCollisionHandler>(out PlayerCollisionHandler player))
        {
            player.ProcessCollision(this);
            IsObtained?.Invoke(this);
        }
    }
}