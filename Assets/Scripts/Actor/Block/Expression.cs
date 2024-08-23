using UnityEngine;

public class Expression : Block
{
    [Header("Expression References")]
    public ExpressionType type;

    public override bool Move(Vector2 direction)
    {
        if (isStatic) return false;
        if (IsMoving) return false;

        // Raycast to perform operations
        var hit = Physics2D.Raycast(transform.position, direction, 1f);
        if (hit)
        {
            var number = hit.transform.GetComponent<Number>();
            if (number && number.Merge(this)) return true;
        }

        return base.Move(direction);
    }
}
