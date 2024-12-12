using UnityEngine;

public class Pin : Interactable
{
    private Animator _animator;
    private static readonly int LockAnimationBool = Animator.StringToHash("isLocked");
    private bool _isLocked;
    public bool IsLocked
    {
        get => _isLocked;
        set
        {
            _isLocked = value;
            _animator.SetBool(LockAnimationBool, value);
        }
    }

    [Header("Pin References")]
    [SerializeField] private bool lockedOnStart = true;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        IsLocked = lockedOnStart;
    }

    #endregion

    public override bool Move(Vector2 direction)
    {
        return Physics2D.OverlapBoxAll(transform.position, Vector2.one * 0.5f, 0f).Length <= 1;
    }

    public override bool OnInteracted(Actor actor)
    {
        return false;
    }

    public void SetLocked(bool value)
    {
        IsLocked = value;
    }
}
