using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ResourceCatcher))]
[RequireComponent(typeof(CatchedResourceHandler))]
public class Weapon : MonoBehaviour
{
    private PlayerInput _playerInput;
    private ResourceCatcher _resourceCatcher;
    private CatchedResourceHandler _resourceCatcherHandler;

    public ResourceCatcher ResourceCatcher => _resourceCatcher;

    public event Action ShootButtonPressed;

    private void Awake()
    {
        _resourceCatcher = GetComponent<ResourceCatcher>();
        _resourceCatcherHandler = GetComponent<CatchedResourceHandler>();

        _playerInput = new PlayerInput();
    }
    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void Start()
    {
        _resourceCatcher.SetWeapon(this);
        _resourceCatcherHandler.SetWeapon(this);
        _playerInput.Player.Gather.performed += ctx => ShootButtonPressed?.Invoke();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }
}
