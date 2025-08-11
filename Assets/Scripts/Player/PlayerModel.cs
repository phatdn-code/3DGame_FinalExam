using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerModel : MonoBehaviour
{
    // --- Public Properties for View to observe ---
    public Vector2 CurrentMoveInput { get; private set; }
    public bool IsAiming { get; private set; }
    public Vector3 Velocity => _controller ? _controller.velocity : Vector3.zero;

    // --- Events for View to subscribe to ---
    public event Action OnJumped;

    // --- Serialized Fields for configuration ---
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 1.4f;
    [SerializeField] private Transform modelTransform; // Reference to the visual model

    // --- Private State ---
    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _jumpRequested = false;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        ApplyJump();
        ApplyMove();

        // Apply final movement
        _controller.Move(_velocity * Time.fixedDeltaTime);
    }

    // --- Public methods for Controller to call ---
    public void SetMoveInput(Vector2 input)
    {
        CurrentMoveInput = input;
    }

    public void RequestJump()
    {
        if (_controller.isGrounded)
        {
            _jumpRequested = true;
        }
    }

    public void SetAiming(bool isAiming)
    {
        IsAiming = isAiming;
    }

    // --- Core Physics Logic ---
    private void ApplyGravity()
    {
        if (_controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -1f; // A small downward force to keep grounded
        }
        else
        {
            _velocity.y += Physics.gravity.y * Time.fixedDeltaTime;
        }
    }

    private void ApplyJump()
    {
        if (!_jumpRequested) return;

        _velocity.y = Mathf.Sqrt(2f * -Physics.gravity.y * jumpHeight);
        _jumpRequested = false;
        OnJumped?.Invoke(); // Notify listeners (the View) that a jump occurred
    }

    private void ApplyMove()
    {
        // Use the camera's transform for direction unless aiming
        // Note: Camera.main can be slow. A direct reference is better.
        Transform moveBase = IsAiming ? modelTransform : Camera.main.transform;
        
        Vector3 direction = moveBase.forward * CurrentMoveInput.y + moveBase.right * CurrentMoveInput.x;
        Vector3 move = direction.normalized * moveSpeed;

        if (_controller.isGrounded)
        {
            _velocity.x = move.x;
            _velocity.z = move.z;
        }
        else // Air control
        {
            // Simple air control for demonstration
            _velocity.x = Mathf.Lerp(_velocity.x, move.x, 0.1f);
            _velocity.z = Mathf.Lerp(_velocity.z, move.z, 0.1f);
        }
    }
}