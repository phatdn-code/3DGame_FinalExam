using UnityEngine;

public class M4 : MonoBehaviour
{
    [SerializeField] private Transform firePosition;
    [SerializeField] private Transform muzzleFlash;
    [SerializeField] private Transform hitEffectHole;
    [SerializeField] private Transform hitEffect;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private int damage;

    [SerializeField] private AudioSource source;
    public void Shoot()
    {
        Ray firingRay = new(firePosition.position, firePosition.forward);
        if (Physics.Raycast(firingRay, out var raycasthit))
        {
            Instantiate(muzzleFlash, firePosition.position, Quaternion.LookRotation(firePosition.forward), firePosition);
            if (raycasthit.collider.TryGetComponent<Health>(out var health))
            {
                health.TakeDamage(damage);
                Instantiate(hitEffect, raycasthit.point, Quaternion.LookRotation(raycasthit.normal), raycasthit.collider.transform);
            }
            else
            {
                Instantiate(hitEffectHole, raycasthit.point, Quaternion.LookRotation(raycasthit.normal), raycasthit.collider.transform);
            }
        }
        source.clip = fireSound;
        source.Play();
    }
}
