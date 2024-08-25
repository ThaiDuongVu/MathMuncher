using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    [Header("UI References")]
    [SerializeField] private SpeechBubble speechBubble;

    private static readonly string[] mathVoiceLines = new string[] { "Math!" };
    private static readonly string[] portalVoiceLines = new string[] { "Yeah!", "Wooo!", "Cool!", "Nice!" };

    private InputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle movement input
        _inputManager.Player.Move.performed += MoveOnPerformed;
        _inputManager.Player.Move.canceled += MoveOnCanceled;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    #endregion

    #region Input Handlers

    private void MoveOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance && GameController.Instance.State != GameState.InProgress) return;

        var direction = context.ReadValue<Vector2>();
        if (direction.x != 0f && direction.y != 0f) return;
        Move(direction);
    }

    private void MoveOnCanceled(InputAction.CallbackContext context)
    {

    }

    #endregion

    #region Talk Methods

    public void Talk(string message)
    {
        speechBubble.SetText(message);
        speechBubble.Intro();
    }

    public void TalkMath()
    {
        Talk(mathVoiceLines[Random.Range(0, mathVoiceLines.Length)]);
    }

    public void TalkPortal()
    {
        Talk(portalVoiceLines[Random.Range(0, portalVoiceLines.Length)]);
    }

    #endregion
}
