using UnityEngine;

public class Grass : Actor
{
    [Header("Grass References")]
    [SerializeField] private Sprite[] sprites;

    private Animator _animator;
    private static readonly int SwayAnimationTrigger = Animator.StringToHash("sway");

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        sprite.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    #endregion

    public override bool Move(Vector2 direction)
    {
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetTrigger(SwayAnimationTrigger);
        }
    }
}
