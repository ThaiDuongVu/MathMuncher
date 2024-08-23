using UnityEngine;

public class Operator : Block
{
    [Header("Operator References")]
    public OperatorType type;

    #region Unity Events

    protected override void Start()
    {
        base.Start();

        SetText(GetSymbol(type));
    }

    #endregion

    public static string GetSymbol(OperatorType type)
    {
        return type switch
        {
            OperatorType.Add => "+",
            OperatorType.Subtract => "-",
            OperatorType.Multiply => "*",
            OperatorType.Divide => "/",
            _ => "",
        };
    }

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
