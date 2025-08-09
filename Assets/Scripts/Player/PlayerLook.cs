using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    private float pitch;

	private void Start()
	{
        pitch = cameraPivot.transform.localEulerAngles.x;
	}

	private void Update()
    {
        var input = lookAction.action.ReadValue<Vector2>();
        UpdateYaw(input.x);
        UpdatePitch(input.y);
    }

    private void UpdateYaw(float inputX)
    {
        var deltaYaw = inputX * rotateSpeed * Time.deltaTime;
        transform.Rotate(0, deltaYaw, 0);
    }

    private void UpdatePitch(float inputY)
    {
        var deltaPitch = -inputY * rotateSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch + deltaPitch, minPitch, maxPitch);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}
