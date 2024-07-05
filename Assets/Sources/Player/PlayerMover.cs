using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed = 6.0f;
    [SerializeField] private float _turnSpeed = 400.0f;
    [SerializeField] private float _gravity;
    [SerializeField] private Transform _planet;
    [SerializeField] private float _raycastDistance = 2.0f;

    private CharacterController _controller;
    private Vector3 _moveDirection = Vector3.zero;
    private PlayerInput _playerInput;
    private Vector2 _inputVector;
    private PlayerAnimator _animator;
    private Transform _transform;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<PlayerAnimator>();
        _playerInput = new PlayerInput();
        _transform = transform;

        _playerInput.Player.Move.performed += OnMove;
        _playerInput.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _inputVector = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _inputVector = Vector2.zero;
    }

    private void Update()
    {
        Vector3 gravityDirection = (_planet.position - transform.position).normalized;

        if (IsGrounded())
        {
            Vector3 forward = _transform.forward * _inputVector.y;
            Vector3 right = _transform.right * _inputVector.x;
            _moveDirection = (forward + right).normalized * _speed;
            _moveDirection += gravityDirection * _gravity * Time.deltaTime;

            if (_inputVector.sqrMagnitude > 0)
            {
                _animator.SetRunningAnimation(true);
            }
            else
            {
                _animator.SetRunningAnimation(false);
            }
        }
        else
        {
            _moveDirection += gravityDirection * 30 * Time.deltaTime;
        }

        _controller.Move(_moveDirection * Time.deltaTime);

        float turn = _inputVector.x * _turnSpeed * Time.deltaTime;
        _transform.Rotate(0, turn, 0);
    }

    private bool IsGrounded()
    {
        Vector3 raycastDirection = (_planet.position - _transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(_transform.position, -raycastDirection, out hit, _raycastDistance))
        {
            return hit.collider != null;
        }

        return false;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }
}


