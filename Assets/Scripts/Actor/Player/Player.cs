using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Actor
{
    [Header("Animator References")]
    [SerializeField] private RuntimeAnimatorController frontAnimator;
    [SerializeField] private RuntimeAnimatorController backAnimator;
    [SerializeField] private RuntimeAnimatorController sideAnimator;
    private Animator _animator;
    private static readonly int IntroAnimationTrigger = Animator.StringToHash("intro");

    [Header("UI References")]
    [SerializeField] private SpeechBubble speechBubble;

    [Header("Audio References")]
    [SerializeField] private AudioSource footstepAudio;

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

    protected virtual void SetAnimatorDirection(Vector2 direction)
    {
        if (!_animator) return;

        _animator.runtimeAnimatorController =
            direction.x == 0f ? (direction.y < 0f ? frontAnimator : backAnimator) : sideAnimator;
    }

    public void Talk(string message)
    {
        speechBubble.SetText(message);
        speechBubble.Intro();
    }

    private bool Enter(Teleporter teleporter)
    {
        if (!teleporter) return false;

        // Teleport
        teleporter.OnActivated(this);

        // Play effects
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        _animator.SetTrigger(IntroAnimationTrigger);

        return true;
    }

    public override bool Move(Vector2 direction)
    {
        if (isStatic) return false;
        if (IsMoving) return false;

        // Raycast to perform operations
        var hit = Physics2D.Raycast(transform.position, direction, 1f);
        if (hit)
        {
            var hitTransform = hit.transform;
            Enter(hitTransform.GetComponent<Teleporter>());
        }

        SetAnimatorDirection(direction);
        footstepAudio.Play();

        return base.Move(direction);
    }
}
