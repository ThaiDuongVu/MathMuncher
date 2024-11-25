using UnityEngine;

public class Gate : MonoBehaviour
{
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
    }
}
