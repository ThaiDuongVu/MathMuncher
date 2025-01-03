using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightBulb : Interactable
{
    [Header("Light Bulb References")]
    [SerializeField] private float revealRadius = 1.5f;
    [SerializeField] private Light2D revealLight;
    [SerializeField] private SpriteMask revealCircle;

    #region Unity Events

    protected override void Start()
    {
        base.Start();

        // Update reveal radius
        revealLight.pointLightInnerRadius = revealLight.pointLightOuterRadius = revealRadius;
        revealCircle.transform.localScale = Vector2.one * revealRadius * 2f;
    }

    #endregion

    public override bool OnInteracted(Actor actor)
    {
        return false;
    }
}
