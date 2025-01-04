using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private bool openOnStart;
    [SerializeField] private AudioSource openAudio;
    private Animator _animator;
    private static readonly int OpenAnimationTrigger = Animator.StringToHash("open");
    private static readonly int CloseAnimationTrigger = Animator.StringToHash("close");

    #region Unity Events

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (openOnStart) Open();
    }

    #endregion

    public void Open()
    {
        _animator.SetTrigger(OpenAnimationTrigger);
        openAudio.Play();
        Level.Instance.SendUIMessage("Gate opened");
    }

    public void Close()
    {
        _animator.SetTrigger(CloseAnimationTrigger);
        openAudio.Play();
        Level.Instance.SendUIMessage("Gate closed");
    }
}
