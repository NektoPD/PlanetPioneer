using System;
using UnityEngine;
using Zenject;


public class PlayerToBasePointerView : MonoBehaviour
{
    private Player _player;

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
        _player.ResourceAddedToBag += ActivatePointer;
        _player.ResourceRemovedFromBag += DiactivatePointer;
    }

    private void Start()
    {
        DiactivatePointer();
    }

    private void OnDestroy()
    {
        _player.ResourceAddedToBag -= ActivatePointer;
        _player.ResourceRemovedFromBag -= DiactivatePointer;
    }

    public void DiactivatePointer()
    {
        gameObject.SetActive(false);
    }

    public void ActivatePointer()
    {
        gameObject.SetActive(true);
    }
}
