using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    public bool isStatic;

    public Vector2 TargetPosition { get; set; }
    private const float MoveSpeed = 20f;
    private const float Epsilon = 0.1f;
    public bool IsMoving { get; set; }

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
        if (IsMoving)
        {
            transform.position = Vector2.Lerp(transform.position, TargetPosition, MoveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, TargetPosition) < Epsilon)
            {
                IsMoving = false;
                transform.position = TargetPosition;
            }
        }
    }

    #endregion

    public void SetFlip(bool value)
    {
        sprite.flipX = value;
    }

    protected void Reactivate()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public virtual bool Move(Vector2 direction)
    {
        if (isStatic) return false;
        if (IsMoving) return false;

        // Raycast to check if movable
        var hit = Physics2D.Raycast(transform.position, direction, 1f);
        if (hit)
        {
            var hitActor = hit.transform.GetComponent<Actor>();
            if (!hitActor) return false;
            if (!hitActor.Move(direction)) return false;
        }

        TargetPosition += direction;
        IsMoving = true;

        // Set sprite horizontal flip accordingly
        if (direction.x < 0f) SetFlip(true);
        else if (direction.x > 0f) SetFlip(false);
        return true;
    }
}
