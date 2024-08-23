using UnityEngine;
using TMPro;

public class Block : Actor
{
    [Header("Block References")]
    [SerializeField] private RectTransform overlay;
    [SerializeField] private TMP_Text text;

    private Camera _mainCamera;
    protected Animator Animator;
    private static readonly int ShrinkAnimationTrigger = Animator.StringToHash("shrink");

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();
        _mainCamera = Camera.main;
        Animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (GameController.Instance)
            overlay.gameObject.SetActive(GameController.Instance.State == GameState.InProgress);
        else if (HomeController.Instance)
            overlay.gameObject.SetActive(!HomeController.Instance.IsInit);

        overlay.position = _mainCamera.WorldToScreenPoint(transform.position);
    }

    #endregion

    public virtual void SetText(string message)
    {
        text.SetText(message);
    }

    public virtual void Shrink()
    {
        Animator.SetTrigger(ShrinkAnimationTrigger);
    }

    private bool Collect(Star star)
    {
        if (!star) return false;

        // Destroy star & reactivate this
        star.OnCollected(this);
        Reactivate();

        // Play effects
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        GameController.Instance.PlaySlowMotionEffect();

        return true;
    }

    private bool Enter(Teleporter teleporter)
    {
        if (!teleporter) return false;

        // Teleport
        teleporter.OnActivated(this);
        Reactivate();

        // Play effects
        CameraShaker.Instance.Shake(CameraShakeMode.Light);

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
            Collect(hitTransform.GetComponent<Star>());
            if (Enter(hitTransform.GetComponent<Teleporter>())) return Move(direction);
        }

        return base.Move(direction);
    }
}
