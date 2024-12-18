using UnityEngine;

public class Key : Actor
{
    [Header("Effects References")]
    [SerializeField] private ParticleSystem splashPrefab;

    private bool Enter(KeyHole keyHole)
    {
        if (!keyHole) return false;

        keyHole.OnActivated();
        Destroy(gameObject);

        // Play effects
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Instantiate(splashPrefab, keyHole.transform.position, Quaternion.identity);

        return true;
    }

    public override bool Move(Vector2 direction)
    {
        if (!CanMove(direction)) return false;

        // Raycast
        var hit = Physics2D.Raycast(transform.position, direction, 1f);
        if (hit)
        {
            // Enter keyhole
            if (Enter(hit.transform.GetComponent<KeyHole>())) return true;
        }

        // moveAudio.Play();
        return base.Move(direction);
    }
}
