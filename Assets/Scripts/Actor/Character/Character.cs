using UnityEngine;
using UnityEngine.Rendering;

public class Character : Actor
{
    [Header("Animator References")]
    [SerializeField] private RuntimeAnimatorController frontAnimator;
    [SerializeField] private RuntimeAnimatorController backAnimator;
    [SerializeField] private RuntimeAnimatorController sideAnimator;
    private Animator _animator;
    private static readonly int IntroAnimationTrigger = Animator.StringToHash("intro");

    [Header("Audio References")]
    [SerializeField] private AudioSource footstepAudio;
    [SerializeField] private AudioSource teleportAudio;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
    }

    #endregion

    protected virtual void SetAnimatorDirection(Vector2 direction)
    {
        if (!_animator) return;

        _animator.runtimeAnimatorController =
            direction.x == 0f ? (direction.y < 0f ? frontAnimator : backAnimator) : sideAnimator;
    }

    private bool Enter(Teleporter teleporter)
    {
        if (!teleporter) return false;

        // Teleport
        teleporter.OnActivated(this);

        // Play effects
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        _animator.SetTrigger(IntroAnimationTrigger);
        teleportAudio.Play();

        return true;
    }

    public override bool Move(Vector2 direction)
    {
        if (isStatic) return false;

        // Raycast to perform operations
        var hit = Physics2D.Raycast(transform.position, direction, 1f);
        if (hit)
        {
            var hitTransform = hit.transform;
            if (Enter(hitTransform.GetComponent<Teleporter>())) return Move(direction);
        }

        SetAnimatorDirection(direction);
        footstepAudio.Play();

        return base.Move(direction);
    }

    public override bool ForceMove(Vector2 position)
    {
        if (!base.ForceMove(position)) return false;

        SetAnimatorDirection((position - (Vector2)transform.position).normalized);
        footstepAudio.Play();

        return true;
    }
}
