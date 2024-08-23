using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SimpleScrollView : MonoBehaviour
{
    [Header("Audio References")]
    [SerializeField] private AudioSource scrollAudio;

    private ScrollRect _scrollRect;
    private float _scrollSpeed;

    private SimpleMenuInputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new SimpleMenuInputManager();

        // Handle scroll input
        _inputManager.UI.Scroll.performed += ScrollOnPerformed;
        _inputManager.UI.Scroll.canceled += ScrollOnCanceled;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    private void Start()
    {
        _scrollSpeed = 0f;
        _scrollRect.verticalNormalizedPosition = 1f;
    }

    private void Update()
    {
        _scrollRect.verticalNormalizedPosition += _scrollSpeed * Time.deltaTime;
    }

    #endregion

    #region Input Handlers

    private void ScrollOnPerformed(InputAction.CallbackContext context)
    {
        _scrollSpeed = context.ReadValue<Vector2>().y * 2f;
        scrollAudio.Play();
    }

    private void ScrollOnCanceled(InputAction.CallbackContext context)
    {
        _scrollSpeed = 0f;
        scrollAudio.Stop();
    }

    #endregion
}
