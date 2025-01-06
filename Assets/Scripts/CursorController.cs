using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    [SerializeField] private LineRenderer moveLine;

    [Header("Arrow References")]
    [SerializeField] private SpriteRenderer arrow;
    [SerializeField] private Sprite arrowUp;
    [SerializeField] private Sprite arrowDown;
    [SerializeField] private Sprite arrowLeft;
    [SerializeField] private Sprite arrowRight;

    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private Vector2 _direction;

    private Camera _mainCamera;
    private Player[] _players;
    private InputActions _inputActions;

    #region Unity Events

    private void OnEnable()
    {
        _inputActions = new InputActions();

        // Handle click input
        _inputActions.Game.Click.performed += ClickOnPerformed;
        _inputActions.Game.Click.canceled += ClickOnCanceled;

        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void Awake()
    {
        _mainCamera = Camera.main;
        _players = FindObjectsByType<Player>(FindObjectsSortMode.None);
    }

    private void Start()
    {
        moveLine.gameObject.SetActive(false);
        arrow.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Update move line
        _endPosition = _mainCamera.ScreenToWorldPoint(Mouse.current.position.value);
        _direction = (_endPosition - _startPosition).normalized;
        moveLine.SetPositions(new Vector3[] { _startPosition, _endPosition });

        // Update arrow sprite
        arrow.transform.position = (_startPosition + _endPosition) / 2f;
        arrow.sprite = GetArrowSprite(NormalizeDirection(_direction));
    }

    #endregion

    #region Input Handlers

    private void ClickOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance && !GameController.Instance.IsInProgress) return;

        moveLine.gameObject.SetActive(true);
        arrow.gameObject.SetActive(true);

        // Set start position
        _startPosition = _mainCamera.ScreenToWorldPoint(Mouse.current.position.value);
    }

    private void ClickOnCanceled(InputAction.CallbackContext context)
    {
        if (GameController.Instance && !GameController.Instance.IsInProgress) return;

        moveLine.gameObject.SetActive(false);
        arrow.gameObject.SetActive(false);

        // Move player(s) based on direction
        foreach (var player in _players)
            if (player) player.Move(NormalizeDirection(_direction));
    }

    #endregion

    private Vector2 NormalizeDirection(Vector2 direction)
    {
        if (direction == Vector2.zero) return Vector2.zero;
        if (Mathf.Abs(direction.x) == Mathf.Abs(direction.y)) return Vector2.zero;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Move horizontally
            if (direction.x > 0) return Vector2.right;
            else return Vector2.left;
        }
        else
        {
            // Move vertically
            if (direction.y > 0) return Vector2.up;
            else return Vector2.down;
        }
    }

    private Sprite GetArrowSprite(Vector2 direction)
    {
        if (direction == Vector2.up) return arrowUp;
        else if (direction == Vector2.down) return arrowDown;
        else if (direction == Vector2.left) return arrowLeft;
        else if (direction == Vector2.right) return arrowRight;
        return null;
    }

    public static void SetCursorEnabled(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value;
    }
}
