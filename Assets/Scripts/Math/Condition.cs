using UnityEngine;

public class Condition : MonoBehaviour
{
    [SerializeField] private OperatorType operatorType;
    [SerializeField] private int value;

    public bool Evaluate(int otherValue)
    {
        return operatorType switch
        {
            OperatorType.GreaterThan => otherValue > value,
            OperatorType.LessThan => otherValue < value,
            OperatorType.Equal => otherValue == value,
            OperatorType.NotEqual => otherValue != value,
            _ => false,
        };
    }

    public override string ToString()
    {
        if (operatorType == OperatorType.None) return "";
        return $"{Operator.GetSymbol(operatorType)} {value}";
    }
}
