using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Actor
{
    [Header("Animator References")]
    [SerializeField] private RuntimeAnimatorController frontAnimator;
    [SerializeField] private AnimatorOverrideController backAnimator;
    [SerializeField] private AnimatorOverrideController sideAnimator;
    [SerializeField] private Animator arrowsDisplay;
    private Animator _animator;
    private static readonly int MoveAnimationTrigger = Animator.StringToHash("move");

    [Header("Effects References")]
    [SerializeField] private ParticleSystem splashPrefab;

    private Turnable[] _turnables;

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

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
        _turnables = FindObjectsByType<Turnable>(FindObjectsSortMode.None);
    }

    #endregion

    #region Input Handlers

    private void MoveOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State != GameState.InProgress) return;

        var direction = context.ReadValue<Vector2>();
        if (direction.x != 0f && direction.y != 0f) return;
        if (!Move(direction)) return;

        // Update turnables
        foreach (var turnable in _turnables) turnable.UpdateTurn(direction);
    }

    private void MoveOnCanceled(InputAction.CallbackContext context)
    {
    }

    #endregion

    public override void Explode()
    {
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        GameController.Instance.PlaySlowMotionEffect();
        GameController.Instance.StartCoroutine(GameController.Instance.GameOver());
        Instantiate(splashPrefab, transform.position, Quaternion.identity);

        base.Explode();
    }

    protected override bool Move(Vector2 direction)
    {
        if (!base.Move(direction)) return false;

        // Set animator based on direction
        // Update arrows display accordingly
        if (direction.x != 0f)
        {
            _animator.runtimeAnimatorController = sideAnimator;
            arrowsDisplay.SetTrigger(Mathf.Approximately(direction.x, 1f) ? "right" : "left");
        }
        else
        {
            _animator.runtimeAnimatorController = direction.y < 0f ? frontAnimator : backAnimator;
            arrowsDisplay.SetTrigger(Mathf.Approximately(direction.y, 1f) ? "up" : "down");
        }

        _animator.SetTrigger(MoveAnimationTrigger);
        return true;
    }
}