using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HomeController : MonoBehaviour
{
    #region Singleton

    private static HomeController _homeControllerInstance;

    public static HomeController Instance
    {
        get
        {
            if (_homeControllerInstance == null) _homeControllerInstance = FindFirstObjectByType<HomeController>();
            return _homeControllerInstance;
        }
    }

    #endregion

    [SerializeField] private SimpleMenu mainMenu;
    [SerializeField] private TMP_Text initText;

    public bool IsInit { get; set; }
    private InputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new InputManager();
        _inputManager.Game.Any.performed += AnyOnPerformed;
        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    #endregion

    #region Input Handlers

    private void AnyOnPerformed(InputAction.CallbackContext context)
    {
        if (!IsInit) Init();
    }

    #endregion

    private void Init()
    {
        mainMenu.SetActive(true);
        initText.gameObject.SetActive(false);
        PostProcessingController.Instance.SetDepthOfField(true);

        IsInit = true;
    }
}
