using UnityEngine;
using TMPro;

public class PassHole : Interactable
{
    [Header("Value References")]
    public int initValue;
    [SerializeField] private TMP_Text valueText;

    [Header("Pass Hole References")]
    [SerializeField] private AudioSource activateAudio;
    [SerializeField] private ParticleSystem splashPrefab;
    [SerializeField] private LineRenderer connectLine;
    [SerializeField] private Transform[] connectedObjects;

    private Animator _animator;
    private static readonly int ActivateAnimationTrigger = Animator.StringToHash("activate");

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
    public bool IsActivated { get; private set; }

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

        // Update connect line
        connectLine.positionCount = connectedObjects.Length * 2;
        var j = 0;
        for (var i = 0; i < connectLine.positionCount; i++)
        {
            if (i % 2 == 0) connectLine.SetPosition(i, transform.position);
            else
            {
                connectLine.SetPosition(i, connectedObjects[j].position);
                j++;
            }
        }
    }

    #endregion

    public override bool OnInteracted(Actor actor)
    {
        // Guard clauses
        if (IsActivated) return false;
        if (!base.OnInteracted(actor)) return false;
        // Only blocks can be eaten
        var block = actor.GetComponent<Block>();
        if (!block) return false;
        // Compare block values
        if (Value != block.Value) return false;

        _animator.SetTrigger(ActivateAnimationTrigger);
        IsActivated = true;

        // Update block
        actor.Teleport(transform.position);
        block.Shrink();
        Destroy(actor.gameObject, 0.5f);

        // Play effects
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        GameController.Instance.PlaySlowMotionEffect();
        Instantiate(splashPrefab, transform.position, Quaternion.identity);
        activateAudio.Play();

        return true;
    }
}
