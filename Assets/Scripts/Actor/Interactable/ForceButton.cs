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

    private readonly Vector2 _boxSize = new(0.5f, 0.5f);
    private bool _isOn;
    public bool IsOn
    {
        get => _isOn;
        set
        {
            _isOn = value;
            sprite.sprite = value ? onSprite : offSprite;
            if (value) onActivated.Invoke();
            else onDeactivated.Invoke();
        }
    }

    #region Unity Events

    protected override void Start()
    {
        base.Start();

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

    protected override void Update()
    {
        base.Update();

        var hits = Physics2D.OverlapBoxAll(transform.position, _boxSize, 0f);
        if (!IsOn && hits.Length > 1)
        {
            IsOn = true;
            CameraShaker.Instance.Shake(CameraShakeMode.Light);
            Instantiate(splashPrefab, transform.position, Quaternion.identity);
        }
        else if (IsOn && hits.Length <= 1)
        {
            IsOn = false;
            CameraShaker.Instance.Shake(CameraShakeMode.Light);
            Instantiate(splashPrefab, transform.position, Quaternion.identity);
        }
    }

    #endregion

    public override bool OnInteracted(Actor actor)
    {
        if (!base.OnInteracted(actor)) return false;

        return false;
    }

    protected override bool Move(Vector2 direction)
    {
        return true;
    }
}
