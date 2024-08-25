using UnityEngine;

public class Condition : MonoBehaviour
{
    [SerializeField] private OperatorType operatorType;
    [SerializeField] private int value;

    public bool Evaluate(int otherValue)
    {
        return operatorType switch
        {
            OperatorType.GreaterThan => value > otherValue,
            OperatorType.LessThan => value < otherValue,
            OperatorType.Equal => value == otherValue,
            OperatorType.NotEqual => value != otherValue,
            _ => false,
        };
    }

    public override string ToString()
    {
        return $"{Operator.GetSymbol(operatorType)} {value}";
    }
}
