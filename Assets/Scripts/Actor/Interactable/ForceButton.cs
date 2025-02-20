using UnityEngine;
using UnityEngine.Events;

public class ForceButton : Interactable
{
    [Header("Switch References")]
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
    [SerializeField] private UnityEvent onActivated;
    [SerializeField] private UnityEvent onDeactivated;

    [Header("Effects References")]
    [SerializeField] private ParticleSystem splashPrefab;
    [SerializeField] private LineRenderer connectLine;
    [SerializeField] private Transform[] connectedObjects;

    [SerializeField] private AudioSource onAudio;
    [SerializeField] private AudioSource offAudio;

    private readonly Vector2 _boxSize = new(0.5f, 0.5f);
    private bool _isOn;
    public bool IsOn
    {
        get => _isOn;
        set
        {
            _isOn = value;
            sprite.sprite = value ? onSprite : offSprite;
            if (value)
            {
                onActivated.Invoke();
                onAudio.Play();
            }
            else
            {
                onDeactivated.Invoke();
                offAudio.Play();
            }
        }
    }

    #region Unity Events

    protected override void Start()
    {
        base.Start();

        connectLine.positionCount = connectedObjects.Length * 2;
    }

    protected override void Update()
    {
        base.Update();

        // Toggle button if something stands on it
        var hits = Physics2D.OverlapBoxAll(transform.position, _boxSize, 0f);
        if (!IsOn && hits.Length > 1)
        {
            IsOn = true;
            PlayEffects();
        }
        else if (IsOn && hits.Length <= 1)
        {
            IsOn = false;
            PlayEffects();
        }

        // Update connect line
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
        if (!base.OnInteracted(actor)) return false;
        return false;
    }

    public override bool Move(Vector2 direction)
    {
        return true;
    }

    private void PlayEffects()
    {
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Instantiate(splashPrefab, transform.position, Quaternion.identity);
    }
}
