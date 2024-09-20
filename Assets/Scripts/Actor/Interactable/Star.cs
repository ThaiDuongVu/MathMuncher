using UnityEngine;

public class Star : Interactable
{
    [Header("Effects References")]
    [SerializeField] private ParticleSystem splashPrefab;

    public override bool OnInteracted(Actor actor)
    {
        if (!base.OnInteracted(actor)) return false;

        actor.Reactivate();

        // Play effects
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Instantiate(splashPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);

        return false;
    }
}
