using UnityEngine;

public class hr_RaycastWeapon : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem metalHitEffect;
    [SerializeField] private ParticleSystem fleshHitEffect;
    [SerializeField] private TrailRenderer tracerEffect;

    [Header("Settings")]
    [SerializeField] private Transform raycastOrigin;

    // Helper variables.
    private Ray ray;
    private RaycastHit hit;

    private GameManager gm;

    void Awake()
    {
        gm = GameManager.GetInstance();
    }

    public void StartFiring()
    {
        if (gm.munitionLeft > 0)
        {
            muzzleFlash.Emit(1);
            hr_AudioManager.instance.Play("gunshot_01");

            ray.origin = raycastOrigin.position;
            ray.direction = raycastOrigin.forward;

            var tracer = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
            tracer.AddPosition(ray.origin);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Zombie"))
                {
                    fleshHitEffect.transform.position = hit.point;
                    fleshHitEffect.transform.forward = hit.normal;
                    fleshHitEffect.Emit(1);

                    hit.collider.GetComponent<hr_ZombieController>().TakeDamage(25.0f);
                }
                else
                {
                    metalHitEffect.transform.position = hit.point;
                    metalHitEffect.transform.forward = hit.normal;
                    metalHitEffect.Emit(1);
                }

                tracer.transform.position = hit.point;
            }
            gm.munitionLeft--;
        }
    }

    public void StopFiring()
    {
    }

}
