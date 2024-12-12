using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Actor
{
    [Header("Player References")]
    [SerializeField] private Animator arrowsDisplay;
    private Animator _animator;
    private static readonly int MoveAnimationTrigger = Animator.StringToHash("move");
    private Skin _currentSkin;

    [Header("Effects References")]
    [SerializeField] private ParticleSystem splashPrefab;
    [SerializeField] private AudioSource moveAudio;

    private Turnable[] _turnables;

    private InputActions _inputActions;

    #region Unity Events

    private void OnEnable()
    {
        _inputActions = new InputActions();

        // Handle movement input
        _inputActions.Player.Move.performed += MoveOnPerformed;
        _inputActions.Player.Move.canceled += MoveOnCanceled;

        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
        _turnables = FindObjectsByType<Turnable>(FindObjectsSortMode.None);
    }

    protected override void Start()
    {
        base.Start();

        // Set skin based on save data
        InitSkin();
        _animator.runtimeAnimatorController = _currentSkin.frontAnimator;
    }

    #endregion

    #region Input Handlers

    private void MoveOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance && !GameController.Instance.IsInProgress) return;

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

    public override bool Move(Vector2 direction)
    {
        var move = base.Move(direction);
        // Set animator based on direction
        // Update arrows display accordingly
        if (direction.x != 0f)
        {
            _animator.runtimeAnimatorController = _currentSkin.sideAnimator;
            arrowsDisplay.SetTrigger(Mathf.Approximately(direction.x, 1f) ? "right" : "left");
        }
        else
        {
            _animator.runtimeAnimatorController = direction.y < 0f ? _currentSkin.frontAnimator : _currentSkin.backAnimator;
            arrowsDisplay.SetTrigger(Mathf.Approximately(direction.y, 1f) ? "up" : "down");
        }
        if (!move) return false;

        _animator.SetTrigger(MoveAnimationTrigger);
        moveAudio.Play();

        return true;
    }

    public void InitSkin()
    {
        var data = SaveLoadController.Instance.LoadPerma();
        _currentSkin = Resources.LoadAll<Skin>("Skins")[data.skinIndex];
    }
}