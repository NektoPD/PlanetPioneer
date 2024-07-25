using System;
using UnityEngine;


public class PlayerToBasePointerView : MonoBehaviour
{
    private Player _player;

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

    public void SetPlayer(Player player)
    {
        if(player == null)
            throw new ArgumentNullException(nameof(player));

        _player = player;
        _player.ResourceAddedToBag += ActivatePointer;
        _player.ResourceRemovedFromBag += DiactivatePointer;
    }
}
