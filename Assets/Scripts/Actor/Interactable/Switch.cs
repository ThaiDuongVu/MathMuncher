using UnityEngine;
using UnityEngine.Events;

public class Switch : Interactable
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

        IsOn = false;
    }

    #endregion

    public override bool OnInteracted(Actor actor)
    {
        if (!base.OnInteracted(actor)) return false;

        IsOn = !IsOn;

        // Play effects
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Instantiate(splashPrefab, transform.position, Quaternion.identity);

        return false;
    }

    protected override bool Move(Vector2 direction)
    {
        return true;
    }
}