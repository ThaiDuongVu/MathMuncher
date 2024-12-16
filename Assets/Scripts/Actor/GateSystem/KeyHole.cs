using UnityEngine;
using UnityEngine.Events;

public class KeyHole : Actor
{
    [Header("Key Hole References")]
    [SerializeField] private UnityEvent activateEvents;
    [SerializeField] private AudioSource activateAudio;
    [SerializeField] private LineRenderer connectLine;
    [SerializeField] private Transform[] connectedObjects;

    private Animator _animator;
    private static readonly int ActivateAnimationTrigger = Animator.StringToHash("activate");

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }

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

    #endregion

    public void OnActivated()
    {
        Reactivate();
        _animator.SetTrigger(ActivateAnimationTrigger);

        activateEvents.Invoke();
        activateAudio.Play();
    }
}
