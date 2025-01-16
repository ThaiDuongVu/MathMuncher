using System.Collections;
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
    public bool IsInit { get; private set; }

    [SerializeField] private Actor[] actors;
    [SerializeField] private Eater eater;

    private InputActions _inputActions;

    #region Unity Events

    private void OnEnable()
    {
        _inputActions = new InputActions();

        // Handle any input
        _inputActions.Mobile.Touch.performed += AnyOnPerformed;

        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private IEnumerator Start()
    {
        Time.timeScale = 1f;

        // Disable all blocks
        foreach (var actor in actors)
            actor.GetComponent<Animator>().speed = 0f;
        yield return new WaitForSeconds(0.25f);

        // Enable blocks one by one
        foreach (var actor in actors)
        {
            yield return new WaitForSeconds(0.125f);
            actor.GetComponent<Animator>().speed = 1f;
        }

        // Play eater animation
        var eaterAnimator = eater.GetComponent<Animator>(); 
        eaterAnimator.SetTrigger("eat");
        yield return new WaitForSeconds(0.5f);
        eaterAnimator.speed = 0f;
    }

    #endregion

    #region Input Handlers

    private void AnyOnPerformed(InputAction.CallbackContext context)
    {
        if (IsInit) return;

        mainMenu.SetActive(true);
        initText.gameObject.SetActive(false);
        PostProcessingController.Instance.SetDepthOfField(true);

        IsInit = true;
    }

    #endregion
}
