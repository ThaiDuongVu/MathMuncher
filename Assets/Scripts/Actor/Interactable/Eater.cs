using UnityEngine;
using TMPro;

public class Eater : Interactable
{
    [Header("Eater References")]
    [SerializeField] private int initValue;
    [SerializeField] private TMP_Text valueText;

    [Header("Effects References")]
    [SerializeField] private ParticleSystem splashPrefab;

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
    private static readonly int EatAnimationTrigger = Animator.StringToHash("eat");

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

    #endregion

    public override bool OnInteracted(Actor actor)
    {
        // Guard clauses
        if (!base.OnInteracted(actor)) return false;
        var block = actor.GetComponent<Block>();
        if (!block) return false;
        if (Value != block.Value) return false;

        _animator.SetTrigger(EatAnimationTrigger);
        Talk("Mmm");

        // Update block
        actor.Teleport(transform.position);
        block.Shrink();
        Destroy(actor.gameObject, 0.5f);

        // Play effects
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        GameController.Instance.PlaySlowMotionEffect();
        Instantiate(splashPrefab, transform.position, Quaternion.identity);

        return true;
    }
}