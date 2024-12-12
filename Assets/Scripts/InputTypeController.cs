using UnityEngine;
using UnityEngine.InputSystem;

public class InputTypeController : MonoBehaviour
{
    #region Singleton

    private static InputTypeController _inputTypeControllerInstance;

    public static InputTypeController Instance
    {
        get
        {
            if (_inputTypeControllerInstance == null)
                _inputTypeControllerInstance = FindFirstObjectByType<InputTypeController>();
            return _inputTypeControllerInstance;
        }
    }

    #endregion

    public InputType InputType { get; private set; } = InputType.MouseKeyboard;

    private InputActions _inputActions;

    #region Unity Event

    private void OnEnable()
    {
        _inputActions = new InputActions();
        _inputActions.Game.Any.performed += AnyOnPerformed;
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void Update()
    {
        if (Mouse.current.delta.ReadValue().magnitude > 0f) InputType = InputType.MouseKeyboard;

        // Enable/disable hardware cursor based on input type
        CursorController.SetCursorEnabled(InputType == InputType.MouseKeyboard);
    }

    #endregion

    #region Input Handlers

    private void AnyOnPerformed(InputAction.CallbackContext context)
    {
        CheckInputType(context);
    }

    #endregion

    private void CheckInputType(InputAction.CallbackContext context)
    {
        InputType = context.control.device == InputSystem.devices[0] || context.control.device == InputSystem.devices[1]
            ? InputType.MouseKeyboard
            : InputType.Gamepad;
    }
}