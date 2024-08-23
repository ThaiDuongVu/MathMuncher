using UnityEngine;

public class Portal : Block
{
    [Header("Portal References")]
    public int value;
    [SerializeField] private ParticleSystem portalSplashPrefab;

    private static readonly int EnterAnimationTrigger = Animator.StringToHash("enter");

    #region Unity Events

    protected override void Start()
    {
        base.Start();

        SetText(value.ToString());
    }

    #endregion

    public void OnEntered(Block block)
    {
        Animator.SetTrigger(EnterAnimationTrigger);
        Instantiate(portalSplashPrefab, transform.position, Quaternion.identity);
    }
}
