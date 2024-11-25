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

    #region Unity Events

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
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

    protected virtual void Talk(string message)
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

    public virtual bool Move(Vector2 direction)
    {
        if (IsMoving) return false;
        if (isStatic) return false;

        // Raycast to check if movable
        var hit = Physics2D.Raycast(transform.position, direction, 1f);
        if (hit)
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

    #endregion
}