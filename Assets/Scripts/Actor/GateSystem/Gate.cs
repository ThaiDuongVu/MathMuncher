using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private bool openOnStart;
    [SerializeField] private AudioSource openAudio;
    private Animator _animator;
    private static readonly int OpenAnimationBool = Animator.StringToHash("isOpened");

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
        _animator.SetBool(OpenAnimationBool, true);
        openAudio.Play();
        Level.Instance.SendUIMessage("Gate opened");
    }

    public void Close()
    {
        _animator.SetBool(OpenAnimationBool, false);
        openAudio.Play();
        Level.Instance.SendUIMessage("Gate closed");
    }
}
