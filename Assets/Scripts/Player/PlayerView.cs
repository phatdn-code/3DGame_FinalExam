using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private PlayerModel model; // Reference to the Model
    [SerializeField] private Animator animator;
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform cameraMainTransform; // Use transform instead of GameObject
    [SerializeField] private GameObject aimMode;

    [Header("Settings")]
    [SerializeField] private float lookSpeed = 150f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float minPitch = -45f;
    [SerializeField] private float maxPitch = 70f;
    
    // --- Animator Hashes (for performance) ---
    private readonly int _animMoveSpeed = Animator.StringToHash("MoveSpeed");
    private readonly int _animJump = Animator.StringToHash("Jump");
    private readonly int _animAim = Animator.StringToHash("Aim");
    private readonly int _animMoveVertical = Animator.StringToHash("MoveVertical");
    private readonly int _animMoveHorizontal = Animator.StringToHash("MoveHorizontal");

    private float _pitch;

    private void OnEnable()
    {
        if (model != null)
        {
            model.OnJumped += HandleJumpAnimation;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        if (model != null)
        {
            model.OnJumped -= HandleJumpAnimation;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Start()
    {
        _pitch = cameraPivot.transform.localEulerAngles.x;
    }

    private void LateUpdate()
    {
        if (model == null) return;

        UpdateAnimations();
        UpdateAimModeVisuals();
        
        if (!model.IsAiming)
        {
            UpdateModelRotation();
        }
    }

    // --- Public method for Controller to call ---
    public void UpdateLook(Vector2 lookInput)
    {
        // Update Yaw (Left/Right Look) - Rotates the entire player body
        transform.Rotate(0, lookInput.x * lookSpeed * Time.deltaTime, 0);

        // Update Pitch (Up/Down Look) - Rotates only the camera pivot
        _pitch = Mathf.Clamp(_pitch - lookInput.y * lookSpeed * Time.deltaTime, minPitch, maxPitch);
        cameraPivot.localRotation = Quaternion.Euler(_pitch, 0, 0);
    }
    
    // --- Responding to Model's state ---
    private void HandleJumpAnimation()
    {
        animator.SetTrigger(_animJump);
    }

    private void UpdateAnimations()
    {
        animator.SetFloat(_animMoveSpeed, model.CurrentMoveInput.magnitude);
        animator.SetBool(_animAim, model.IsAiming);
        
        if (model.IsAiming)
        {
            // Update blend tree for strafing animations
            animator.SetFloat(_animMoveHorizontal, model.CurrentMoveInput.x);
            animator.SetFloat(_animMoveVertical, model.CurrentMoveInput.y);
        }
    }

    private void UpdateModelRotation()
    {
        // When not aiming, the model should face the direction of movement
        if (model.CurrentMoveInput != Vector2.zero)
        {
            Vector3 camForward = Vector3.ProjectOnPlane(cameraMainTransform.forward, Vector3.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(cameraMainTransform.right, Vector3.up).normalized;
            Vector3 moveDir = camForward * model.CurrentMoveInput.y + camRight * model.CurrentMoveInput.x;

            Transform modelTransform = animator.transform;
            modelTransform.rotation = Quaternion.Lerp(modelTransform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * turnSpeed);
        }
    }

    private void UpdateAimModeVisuals()
    {
        bool isAiming = model.IsAiming;
        if (aimMode.activeSelf != isAiming)
        {
            aimMode.SetActive(isAiming);
            cameraMainTransform.gameObject.SetActive(!isAiming);
        }

        // When aiming, the model should always face where the camera is looking
        // if (isAiming)
        // {
        //     animator.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        // }
    }
}