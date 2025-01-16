using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;

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
    private Vector2 _touchPosition;

    private Camera _mainCamera;
    private Player[] _players;
    private InputActions _inputActions;

    #region Unity Events

    private void OnEnable()
    {
        _inputActions = new InputActions();

        // Handle touch position
        _inputActions.Mobile.TouchPosition.performed += (InputAction.CallbackContext context) => { _touchPosition = context.ReadValue<Vector2>(); };
        // Handle touch input
        _inputActions.Mobile.Touch.performed += TouchOnPerformed;
        _inputActions.Mobile.Touch.canceled += TouchOnCanceled;

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
        _endPosition = _mainCamera.ScreenToWorldPoint(_touchPosition);
        _direction = (_endPosition - _startPosition).normalized;
        moveLine.SetPositions(new Vector3[] { _startPosition, _endPosition });

        // Update arrow sprite
        arrow.transform.position = (_startPosition + _endPosition) / 2f;
        arrow.sprite = GetArrowSprite(NormalizeDirection(_direction));
    }

    #endregion

    #region Input Handlers

    private void TouchOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance && !GameController.Instance.IsInProgress) return;

        StartCoroutine(StartMove());
    }

    private IEnumerator StartMove()
    {
        yield return new WaitForEndOfFrame();

        moveLine.gameObject.SetActive(true);
        arrow.gameObject.SetActive(true);

        // Set start position
        _startPosition = _mainCamera.ScreenToWorldPoint(_touchPosition);
    }

    private void TouchOnCanceled(InputAction.CallbackContext context)
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
