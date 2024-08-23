using UnityEngine;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] private RectTransform overlay;
    [SerializeField] private TMP_Text text;

    private Camera _mainCamera;
    private Animator _animator;
    private static readonly int IntroAnimationTrigger = Animator.StringToHash("intro");

    #region Unity Events

    private void Awake()
    {
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        overlay.gameObject.SetActive(GameController.Instance.State == GameState.InProgress);
        overlay.position = _mainCamera.WorldToScreenPoint(transform.position);
    }

    #endregion

    public virtual void SetText(string message)
    {
        text.SetText(message);
    }

    public void Intro()
    {
        _animator.SetTrigger(IntroAnimationTrigger);
    }
}
