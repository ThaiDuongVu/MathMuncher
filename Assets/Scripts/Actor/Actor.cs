using UnityEngine;

public class Actor : MonoBehaviour
{
    public bool isStatic;
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] private SpeechBubble speechBubble;

    public Vector2 TargetPosition { get; set; }
    public bool IsMoving { get; set; }
    private const float MoveSpeed = 20f;
    private const float Epsilon = 0.1f;
    protected float CastDistance = 1f;
    protected Vector2 CastSize;

    protected BoxCollider2D _boxCollider;

    #region Unity Events

    protected virtual void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Start()
    {
        CastSize = _boxCollider.size;
        TargetPosition = transform.position;
    }

    protected virtual void Update()
    {
        if (!IsMoving) return;

        transform.position = Vector2.Lerp(transform.position, TargetPosition, MoveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, TargetPosition) < Epsilon)
        {
            IsMoving = false;
            transform.position = TargetPosition;
        }
    }

    #endregion

    #region Setters

    protected void SetFlipDirection(Vector2 direction)
    {
        if (!sprite) return;
        switch (direction.x)
        {
            // Set sprite horizontal flip accordingly
            case < 0f:
                sprite.flipX = true;
                break;
            case > 0f:
                sprite.flipX = false;
                break;
        }
    }

    #endregion

    public void Reactivate()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public virtual void Talk(string message)
    {
        speechBubble.gameObject.SetActive(false);
        speechBubble.SetText(message);
        speechBubble.gameObject.SetActive(true);
    }

    #region Teleport Methods

    public virtual void Teleport(Vector2 position)
    {
        transform.position = position;
        TargetPosition = position;
    }

    private bool Enter(Teleporter teleporter, Vector2 direction)
    {
        if (!teleporter) return false;

        var originalPosition = transform.position;
        teleporter.OnTeleported(this);
        // If teleported successfully
        if (Move(direction))
        {
            Reactivate();
            teleporter.PlayEffects();
            return true;
        }

        // If not teleport back to original position
        Teleport(originalPosition);
        return false;
    }

    #endregion

    private void Hatch(Chicken chicken)
    {
        if (!chicken) return;
        StartCoroutine(chicken.OnHatched(this));
    }

    public virtual void Explode()
    {
        Destroy(gameObject);
    }

    #region Move Methods

    // Returns whether a move is successfully performed
    public virtual bool Move(Vector2 direction)
    {
        if (!CanMove(direction)) return false;

        // Cast to check if movable
        // var hit = Physics2D.Raycast(transform.position, direction, RaycastDistance);
        var hits = Physics2D.BoxCastAll(transform.position, CastSize, 0f, direction, CastDistance);
        foreach (var hit in hits)
        {
            var actor = hit.transform.GetComponent<Actor>();
            if (!actor) return false;

            // Teleport
            if (Enter(hit.transform.GetComponent<Teleporter>(), direction)) return true;
            // Hatch
            Hatch(hit.transform.GetComponent<Chicken>());

            if (!actor.Move(direction)) return false;
        }

        TargetPosition += direction;
        IsMoving = true;
        SetFlipDirection(direction);
        return true;
    }

    public virtual bool CanMove(Vector2 direction)
    {
        // Guard clauses
        if (IsMoving) return false;
        if (isStatic) return false;
        if (direction == Vector2.zero) return false;

        // If actor is pinned
        var overlaps = Physics2D.OverlapBoxAll(transform.position, Vector2.one * 0.5f, 0f);
        if (overlaps.Length > 1)
        {
            foreach (var overlap in overlaps)
                if (overlap.CompareTag("Pin")) return false;
        }

        return true;
    }

    #endregion
}