using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    private const float UpgradedMovementSpeed = 3f;

    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _turnSpeed = 400f;
    [SerializeField] private float _joysticTurnSpeed = 300f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private FixedJoystick _joystick;

    private Rigidbody _rigidbody;
    private Vector3 _moveDirection = Vector3.zero;
    private PlayerInput _playerInput;
    private Vector2 _inputVector;
    private PlayerAnimator _animator;
    private Transform _transform;
    private IPlayerUpgrader _playerUpgrader;
    private IResourceGatherer _resourceGatherer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<PlayerAnimator>();
        _playerInput = new PlayerInput();
        _transform = transform;

        _rigidbody.useGravity = false;
        _rigidbody.freezeRotation = true;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        _playerInput.Player.Move.performed += OnMove;
        _playerInput.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnEnable()
    {
        EnableMovement();
    }

    private void OnDisable()
    {
        DisableMovement();

        _playerUpgrader.UpgradedPlayerMovingSpeed -= UpgradeMovementSpeed;
        _resourceGatherer.StartedGatheringResources -= DisableMovement;
        _resourceGatherer.StopedGatheringResources -= EnableMovement;
    }

    private void Update()
    {
        HandleMovement();
    }

    public void EnableMovement()
    {
        _playerInput.Enable();
    }

    public void DisableMovement()
    {
        _playerInput.Disable();
    }

    public void SetPlayerUpgrader(IPlayerUpgrader upgrader)
    {
        if (upgrader == null)
            throw new ArgumentNullException(nameof(upgrader));

        _playerUpgrader = upgrader;
        _playerUpgrader.UpgradedPlayerMovingSpeed += UpgradeMovementSpeed;
    }

    public void SetResourceGatherer(IResourceGatherer resourceGatherer)
    {
        if (resourceGatherer == null)
            throw new ArgumentNullException(nameof(resourceGatherer));

        _resourceGatherer = resourceGatherer;
        _resourceGatherer.StartedGatheringResources += DisableMovement;
        _resourceGatherer.StopedGatheringResources += EnableMovement;
    }

    private void HandleMovement()
    {
        Vector3 forward = _transform.forward * (_inputVector.y + _joystick.Vertical);
        Vector3 right = _transform.right * (_inputVector.x + _joystick.Horizontal);
        _moveDirection = (forward + right).normalized * _speed;
        
        _rigidbody.velocity = _moveDirection;

        if (_moveDirection.sqrMagnitude == 0)
        {
            _rigidbody.velocity = Vector3.zero;
        }
        else
        {
            _rigidbody.velocity = _moveDirection;
        }

        _animator.SetRunningAnimation(_moveDirection.sqrMagnitude > 0);

        float turn = (_inputVector.x * _turnSpeed + _joystick.Horizontal * _joysticTurnSpeed) * Time.deltaTime;
        _transform.Rotate(0, turn, 0);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _inputVector = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _inputVector = Vector2.zero;
    }

    private void UpgradeMovementSpeed()
    {
        _speed = UpgradedMovementSpeed;
    }
}