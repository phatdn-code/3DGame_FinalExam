using System;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 1.4f;
    [SerializeField] private Transform modelTransform;
    public static event Action OnJumpStarted;

    private CharacterController controller;
    private Vector3 moveInput;
    private Vector3 velocity;
    private bool isJumping = false;
    private bool isAiming;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        PlayerController.OnMoveInput += HandleMoveInput;
        PlayerController.OnJumpRequested += HandleJumpRequest;
        PlayerController.OnAimInput += HandleAimInput;
    }

    private void OnDisable()
    {
        PlayerController.OnMoveInput -= HandleMoveInput;
        PlayerController.OnJumpRequested -= HandleJumpRequest;
        PlayerController.OnAimInput -= HandleAimInput;
    }

    private void HandleMoveInput(Vector2 input)
    {
        Transform moveBase = isAiming ? modelTransform : Camera.main.transform;
        var direction = moveBase.forward * input.y + moveBase.right * input.x;
        moveInput = direction.normalized * moveSpeed;
    }

    private void HandleJumpRequest()
    {
        if (controller.isGrounded)
            isJumping = true;
    }

    private void HandleAimInput(bool aimInput) => isAiming = aimInput;

    private void FixedUpdate()
    {
        velocity = controller.velocity;
        ApplyGravity();
        ApplyJump();
        ApplyMove();
        controller.Move(velocity * Time.fixedDeltaTime);
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded)
            velocity.y = -1f;
        else
            velocity.y += Physics.gravity.y * Time.fixedDeltaTime;
    }

    private void ApplyJump()
    {
        if (!isJumping) return;
        velocity.y = Mathf.Sqrt(2f * -Physics.gravity.y * jumpHeight);
        isJumping = false;
        OnJumpStarted?.Invoke();
    }

    private void ApplyMove()
    {
        if (controller.isGrounded)
        {
            velocity.x = moveInput.x;
            velocity.z = moveInput.z;
        }
        else
        {
            var airControl = moveInput * 0.5f;
            velocity.x = Mathf.Lerp(velocity.x, airControl.x, 0.1f);
            velocity.z = Mathf.Lerp(velocity.z, airControl.z, 0.1f);
        }
    }
}

