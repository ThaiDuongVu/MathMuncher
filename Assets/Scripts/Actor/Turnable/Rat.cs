using UnityEngine;

public class Rat : Turnable
{
    [Header("Animator References")]
    [SerializeField] private RuntimeAnimatorController frontAnimator;
    [SerializeField] private AnimatorOverrideController backAnimator;
    [SerializeField] private AnimatorOverrideController sideAnimator;
    private Animator _animator;

    [Header("Position References")] [SerializeField]
    private Vector2[] positions;

    [SerializeField] private LineRenderer positionLine;
    private int _positionIndex;

    [SerializeField] private AudioSource explosionAudio;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();

        positionLine.positionCount = positions.Length;
        for (int i = 0; i < positions.Length; i++)
        {
            var position = positions[i];
            positionLine.SetPosition(i, position);
        }
    }

    #endregion

    protected override bool Move(Vector2 direction)
    {
        return true;
    }

    public override void UpdateTurn(Vector2 direction)
    {
        base.UpdateTurn(direction);

        // Update index
        if (_positionIndex < positions.Length - 1) _positionIndex++;
        else _positionIndex = 0;

        // Move to new position
        var newDirection = (positions[_positionIndex] - (Vector2)transform.position).normalized;
        TargetPosition = positions[_positionIndex];
        IsMoving = true;
        SetFlipDirection(newDirection);

        // Set animator based on direction
        if (newDirection.x != 0f) _animator.runtimeAnimatorController = sideAnimator;
        else _animator.runtimeAnimatorController = newDirection.y < 0f ? frontAnimator : backAnimator;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var actor = other.GetComponent<Actor>();
        if (!actor || actor.isStatic) return;

        actor.Explode();
        Talk("Mmm");
        explosionAudio.Play();
    }
}