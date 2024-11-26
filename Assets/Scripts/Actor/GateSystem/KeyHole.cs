using UnityEngine;
using UnityEngine.Events;

public class KeyHole : Actor
{
    [Header("Key Hole References")]
    [SerializeField] private UnityEvent activateEvents;
    [SerializeField] private AudioSource activateAudio;

    private Animator _animator;
    private readonly int ActivateAnimationTrigger = Animator.StringToHash("activate");

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
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
