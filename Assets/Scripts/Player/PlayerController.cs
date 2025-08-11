using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private PlayerModel model; // Reference to the Model
    [SerializeField] private PlayerView view;   // Reference to the View

    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference aimAction;
    [SerializeField] private InputActionReference lookAction;

    private void Update()
    {
        if (model == null || view == null) return;
        
        // --- Read input and send commands ---

        // Tell the Model how to move
        model.SetMoveInput(moveAction.action.ReadValue<Vector2>());

        // Tell the Model to try jumping
        if (jumpAction.action.triggered)
        {
            model.RequestJump();
        }

        // Tell the Model if we are aiming
        model.SetAiming(aimAction.action.IsPressed());
        
        // Tell the View how to orient the camera
        view.UpdateLook(lookAction.action.ReadValue<Vector2>());
    }
}