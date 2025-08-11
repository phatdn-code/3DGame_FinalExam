using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class AutomaticShooting : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private float cooldown;
    [SerializeField] private GameObject crossHair;
    [SerializeField] private Transform firePosition;

    public UnityEvent OnShoot;
    private float lastShotTime;

    private void Update()
    {
        if (shootAction.action.IsPressed() && FinishCooldown())
        {
            Shoot();
            animator.SetBool("AutoShoot", true);
            lastShotTime = Time.time;
        }
        if (shootAction.action.IsPressed() == false)
        {
            animator.SetBool("AutoShoot", false);
        }
    }

    private bool FinishCooldown() => Time.time - lastShotTime >= cooldown;

    private void Shoot() => OnShoot.Invoke();

    void FixedUpdate()
    {
        Ray aimRay = new Ray(firePosition.position, firePosition.forward);
        if (Physics.Raycast(aimRay, out var raycastHit))
        {
            crossHair.transform.position = Camera.main.WorldToScreenPoint(raycastHit.point);
        }
    }
}
