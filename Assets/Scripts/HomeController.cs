using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HomeController : MonoBehaviour
{
    [SerializeField] private SimpleMenu mainMenu;
    [SerializeField] private TMP_Text initText;
    private bool _isInit;

    private InputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle any input
        _inputManager.Game.Any.performed += AnyOnPerformed;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }
    
    private void Start()
    {
        Time.timeScale = 1f;
    }

    #endregion

    #region Input Handlers

    private void AnyOnPerformed(InputAction.CallbackContext context)
    {
        if (_isInit) return;

        mainMenu.SetActive(true);
        initText.gameObject.SetActive(false);
        PostProcessingController.Instance.SetDepthOfField(true);

        _isInit = true;
    }

    #endregion
}
