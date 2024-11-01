using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SimpleMenu : MonoBehaviour
{
    [SerializeField] private bool disableOnStart;

    [Header("Audio References")]
    [SerializeField] private AudioSource selectAudio;
    [SerializeField] private AudioSource clickAudio;

    private SimpleButton[] _buttons;

    private EventSystem _eventSystem;
    private GameObject _currentSelectedButton;

    private SimpleMenuInputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new SimpleMenuInputManager();
        _inputManager.UI.Enter.performed += EnterOnPerformed;
        _inputManager.Enable();

        if (!_eventSystem.firstSelectedGameObject) StartCoroutine(SelectFirstButton());
        _currentSelectedButton = _eventSystem.currentSelectedGameObject;
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    private void Awake()
    {
        _buttons = GetComponentsInChildren<SimpleButton>();
        _eventSystem = EventSystem.current;
    }

    private void Start()
    {
        SetActive(!disableOnStart);
    }

    private void Update()
    {
        if (_eventSystem.currentSelectedGameObject != _currentSelectedButton)
        {
            _currentSelectedButton = _eventSystem.currentSelectedGameObject;
        }
    }

    #endregion

    #region Input Handlers

    private void EnterOnPerformed(InputAction.CallbackContext context)
    {
        _eventSystem.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        clickAudio.Play();
    }

    #endregion

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    private IEnumerator SelectFirstButton()
    {
        yield return new WaitForEndOfFrame();

        SetSelected(null);
        SetSelected(_buttons[0].gameObject);
    }

    public void SetSelected(GameObject obj)
    {
        _eventSystem.SetSelectedGameObject(obj);
        selectAudio.Play();
    }

    public void SetButtonMainText(int buttonIndex, string text)
    {
        _buttons[buttonIndex].SetMainText(text);
    }

    public void SetButtonSubText(int buttonIndex, string text)
    {
        _buttons[buttonIndex].SetSubText(text);
    }

    public void SetButtonIcon(int buttonIndex, Sprite icon)
    {
        _buttons[buttonIndex].SetIcon(icon);
    }
}
