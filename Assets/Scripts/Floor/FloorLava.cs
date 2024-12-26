using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FloorLava : Floor
{
    [Header("Effects References")]
    [SerializeField] private AudioSource explosionAudio;
    [SerializeField] private new Light2D light;

    #region Unity Events

    protected override void Start()
    {
        base.Start();
        var size = mainSprite.size;
        light.SetShapePath(new Vector3[] {
            new(-size.x / 2f, size.y / 2f),
            new(size.x / 2f, size.y / 2f),
            new(size.x / 2f, -size.y / 2f),
            new(-size.x / 2f, -size.y / 2f),
         });
    }

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
