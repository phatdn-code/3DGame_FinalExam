using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference aimAction;

    public static event Action<Vector2> OnMoveInput;
    public static event Action OnJumpRequested;
    // public static event Action<Vector2> OnLookInput;
    public static event Action<bool> OnAimInput;

    private void Update()
    {
        var moveInput = moveAction.action.ReadValue<Vector2>();
        OnMoveInput?.Invoke(moveInput);

        if (jumpAction.action.triggered)
            OnJumpRequested?.Invoke();

        OnAimInput?.Invoke(aimAction.action.IsPressed());
    }
}
