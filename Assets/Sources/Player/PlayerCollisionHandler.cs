using System;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    public event Action CollidedWithTheBase;
    public event Action CollidedWithGold;

    public void ProcessCollision(IInteractable interactable)
    {
        switch (interactable)
        {
            case BaseSellingSystem:
                CollidedWithTheBase?.Invoke();
                break;
            case Gold:
                CollidedWithGold?.Invoke();
                break;
        }
    }
}