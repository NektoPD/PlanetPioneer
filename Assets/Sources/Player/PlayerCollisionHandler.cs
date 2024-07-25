using System;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{ 
    public event Action CollidedWithTheBase;
    public event Action CollidedWithGold;

    public void ProcessCollision(IInteractable interactable)
    {
        if(interactable is BaseSellingSystem)
        {
            CollidedWithTheBase?.Invoke();
        }
        else if(interactable is Gold)
        {
            CollidedWithGold?.Invoke();
        }
    }
}
