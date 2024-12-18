using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private AudioSource openAudio;
    private Animator _animator;
    private static readonly int OpenAnimationTrigger = Animator.StringToHash("open");

    #region Unity Events

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    #endregion

    public void Open()
    {
        _animator.SetTrigger(OpenAnimationTrigger);
        openAudio.Play();
        Level.Instance.SendUIMessage("Gate opened");
    }
}
