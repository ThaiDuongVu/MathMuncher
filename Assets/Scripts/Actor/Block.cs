using TMPro;
using UnityEngine;

public class Block : Actor
{
    [Header("Block References")]
    public int initValue;
    [SerializeField] private TMP_Text valueText;

    [Header("Effects References")]
    [SerializeField] private ParticleSystem splashPrefab;
    [SerializeField] private AudioSource moveAudio;
    [SerializeField] private AudioSource mergeAudio;

    [Header("Chain References")]
    [SerializeField] private Block chainedBlock;
    [SerializeField] private LineRenderer chain;

    private int _value;
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            valueText.SetText(value.ToString());
        }
    }

    private Animator _animator;
    private static readonly int ShrinkAnimationTrigger = Animator.StringToHash("shrink");
    private static readonly int WiggleAnimationTrigger = Animator.StringToHash("wiggle");

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();

        Value = initValue;
    }

    protected override void Update()
    {
        base.Update();

        if (chainedBlock)
        {
            chain.positionCount = 2;
            chain.SetPosition(0, transform.position);
            chain.SetPosition(1, chainedBlock.transform.position);
        }
        else
            chain.positionCount = 0;
    }

    #endregion

    public override void Explode()
    {
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        GameController.Instance.PlaySlowMotionEffect();
        Instantiate(splashPrefab, transform.position, Quaternion.identity);

        base.Explode();
    }

    public void Shrink()
    {
        _animator.SetTrigger(ShrinkAnimationTrigger);
    }

    public virtual void Merge(Block other)
    {
        if (!other) return;
        if (other == chainedBlock) return;

        Value += other.Value;
        Reactivate();
        Destroy(other.gameObject);

        // Play effects
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Instantiate(splashPrefab, other.transform.position, Quaternion.identity);
        mergeAudio.Play();
    }

    public virtual bool Merge(Operator @operator)
    {
        if (!@operator) return false;

        Value = @operator.Operate(Value);
        Reactivate();
        Destroy(@operator.gameObject);

        // Play effects
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Instantiate(splashPrefab, @operator.transform.position, Quaternion.identity);
        mergeAudio.Play();

        return true;
    }

    public override bool Move(Vector2 direction)
    {
        if (chainedBlock && Vector2.Distance((Vector2)transform.position + direction, chainedBlock.transform.position) >= 1.5f) 
        {
            _animator.SetTrigger(WiggleAnimationTrigger);
            CameraShaker.Instance.Shake(CameraShakeMode.Light);
            return false;
        }
        if (!CanMove(direction)) return false;

        // Raycast
        var hit = Physics2D.Raycast(transform.position, direction, 1f);
        if (hit)
        {
            // Merge & enter
            Merge(hit.transform.GetComponent<Operator>());
            Merge(hit.transform.GetComponent<Block>());

            // Interact
            var interactable = hit.transform.GetComponent<Interactable>();
            if (interactable && interactable.OnInteracted(this)) return true;
        }

        moveAudio.Play();
        return base.Move(direction);
    }
}