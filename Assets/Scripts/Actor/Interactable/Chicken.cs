using System.Collections;
using UnityEngine;

public class Chicken : Actor
{
    [Header("Chicken Stats")]
    [SerializeField] private float hatchRecoveryTime;

    [Header("Chicken References")]
    [SerializeField] private Block blockPrefab;
    [SerializeField] private ParticleSystem splashPrefab;
    [SerializeField] private AudioSource hatchAudio;

    private bool _canHatch = true;

    private Animator _animator;
    private static readonly int HatchAnimationTrigger = Animator.StringToHash("hatch");

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }

    #endregion

    #region Hatch Methods

    public IEnumerator OnHatched(Actor actor)
    {
        if (!_canHatch) yield break;
        if (!actor) yield break;

        _animator.SetTrigger(HatchAnimationTrigger);
        var block = Instantiate(blockPrefab, transform.position, Quaternion.identity);

        // Play effects
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Instantiate(splashPrefab, transform.position, Quaternion.identity);
        Talk("Brr");
        hatchAudio.Play();

        yield return new WaitForEndOfFrame();
        if (!block.Move(sprite.flipX ? Vector2.right : Vector2.left)) Destroy(block.gameObject);

        // Recover
        _canHatch = false;
        Invoke(nameof(RecoverHatch), hatchRecoveryTime);
    }

    private void RecoverHatch()
    {
        _canHatch = true;
    }

    #endregion
}
