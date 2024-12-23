using UnityEngine;

public class FloorLava : Floor
{
    [Header("Effects References")]
    [SerializeField] private AudioSource explosionAudio;

    #region Unity Events

    private void Update()
    {
        // Destroy things
        var hits = Physics2D.OverlapBoxAll(transform.position, mainSprite.size, 0f);
        foreach (var hit in hits)
        {
            var actor = hit.GetComponent<Actor>();
            if (!actor || actor.isStatic) continue;
            actor.Explode();
            explosionAudio.Play();
        }
    }

    #endregion
}
