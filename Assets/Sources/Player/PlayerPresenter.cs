using System;
using UnityEngine;

public class PlayerPresenter : MonoBehaviour
{
    private CatchedResourceHandler _resourceHandler;
    private PlayerResourcesView _resourcesView;

    private void Awake()
    {
        _resourceHandler = GetComponentInChildren<CatchedResourceHandler>();
        _resourcesView = GetComponentInChildren<PlayerResourcesView>();
    }

    private void Start()
    {
        if(_resourceHandler == null)
            throw new ArgumentNullException(nameof(_resourceHandler));

        _resourcesView.SetResourceHandler(_resourceHandler);
    }
}
