using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;
    private readonly int animMoveSpeed = Animator.StringToHash("MoveSpeed");
    private readonly int animJump = Animator.StringToHash("Jump");
    private readonly int animAim = Animator.StringToHash("Aim");
    private readonly int animMoveVertical = Animator.StringToHash("MoveVertical");
    private readonly int animMoveHorizontal = Animator.StringToHash("MoveHorizontal");

    [Header("Rotation Model")]
    [SerializeField] private Transform cameraMain;
    private Transform model;
    private float turnSpeed = 10f;

    [Header("Aim mode")]
    [SerializeField] private GameObject aimMode;

    // Handle cache
    private bool jumpThisFrame;
    private Vector2 moveInput;
    private bool aimInput;

    private void Awake()
    {
        model = animator.transform;
    }

    private void OnEnable()
    {
        PlayerPhysics.OnJumpStarted += HandleJump;
        PlayerController.OnMoveInput += HandleMoveInput;
        PlayerController.OnAimInput += HandleAimInput;
    }

    private void OnDisable()
    {
        PlayerPhysics.OnJumpStarted -= HandleJump;
        PlayerController.OnMoveInput -= HandleMoveInput;
        PlayerController.OnAimInput -= HandleAimInput;
    }

    private void HandleJump() => jumpThisFrame = true;
    private void HandleMoveInput(Vector2 input) => moveInput = input;
    private void HandleAimInput(bool isAiming) => aimInput = isAiming;

    private void LateUpdate()
    {
        ApplyAnimation();
        if (aimInput)
        {
            EnterAimMode();
        }
        else
        {
            ExitAimMode();
            ApplyRotationModel();
        }
    }

    private void ApplyAnimation()
    {
        // animator.SetFloat(animMoveSpeed, velocityInput.magnitude);
        animator.SetFloat(animMoveSpeed, moveInput.magnitude);
        if (jumpThisFrame)
        {
            animator.SetTrigger(animJump);
            jumpThisFrame = false;
        }
        animator.SetBool(animAim, aimInput);
        if (aimInput)
        {
            animator.SetFloat(animMoveHorizontal, moveInput.x);
            animator.SetFloat(animMoveVertical, moveInput.y);
        }
    }

    private void ApplyRotationModel()
    {
        if (moveInput != Vector2.zero)
        {
            Vector3 camForward = Vector3.ProjectOnPlane(cameraMain.forward, Vector3.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(cameraMain.right, Vector3.up).normalized;
            Vector3 moveDir = camForward * moveInput.y + camRight * moveInput.x;
            model.rotation = Quaternion.Lerp(model.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * turnSpeed);
        }
    }

    private void EnterAimMode()
    {
        if (!aimMode.activeSelf)
            aimMode.SetActive(true);
        cameraMain.gameObject.SetActive(false);
    }

    private void ExitAimMode()
    {
        if (aimMode.activeSelf)
            aimMode.SetActive(false);
        cameraMain.gameObject.SetActive(true);
    }
}
