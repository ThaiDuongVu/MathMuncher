using UnityEngine;
using TMPro;

public class Number : Block
{
    [Header("Number References")]
    public int initValue;

    private int _value;
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            SetText(value.ToString());
        }
    }

    [SerializeField] private TMP_Text operatorText;
    private OperatorType _operatorType;
    public OperatorType OperatorType
    {
        get => _operatorType;
        set
        {
            _operatorType = value;
            operatorText.SetText(Operator.GetSymbol(value));
        }
    }

    [SerializeField] private ParticleSystem splashPrefab;

    public bool IsOperatorSet => OperatorType != OperatorType.None;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        Value = initValue;
        OperatorType = OperatorType.None;
    }

    #endregion

    private static int Factorial(int value)
    {
        var result = 1;
        for (var i = 1; i <= value; i++)
            result *= i;

        return result;
    }

    #region Operation Methods

    public bool Merge(Operator @operator)
    {
        if (!@operator) return false;

        // Set current operator type
        OperatorType = @operator.type;

        // Destroy operator & reactivate this
        Destroy(@operator.gameObject);
        Reactivate();

        // Play effects
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Instantiate(splashPrefab, @operator.transform.position, Quaternion.identity);

        return true;
    }

    public bool Merge(Expression expression)
    {
        if (!expression) return false;

        // Handle undefined value
        if (Value < 0)
        {
            if (expression.type == ExpressionType.SquareRoot) return false;
            if (expression.type == ExpressionType.Factorial) return false;
        }

        // Update value
        Value = expression.type switch
        {
            ExpressionType.Square => Value * Value,
            ExpressionType.SquareRoot => Mathf.RoundToInt(Mathf.Sqrt(Value)),
            ExpressionType.Factorial => Factorial(Value),
            ExpressionType.AbsoluteValue => Mathf.Abs(Value),
            _ => Value,
        };

        // Destroy operator & reactivate this
        Destroy(expression.gameObject);
        Reactivate();

        // Play effects
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Instantiate(splashPrefab, expression.transform.position, Quaternion.identity);
        FindFirstObjectByType<Player>().Talk("Math!");

        return true;
    }

    private bool Operate(Number number)
    {
        if (!number) return false;
        if (!IsOperatorSet && !number.IsOperatorSet) return false;

        // Handle undefined value
        if (number.Value == 0 && OperatorType == OperatorType.Divide) return false;

        // Set new value & reset operator type
        Value = (IsOperatorSet ? OperatorType : number.OperatorType) switch
        {
            OperatorType.Add => Value + number.Value,
            OperatorType.Subtract => Value - number.Value,
            OperatorType.Multiply => Value * number.Value,
            OperatorType.Divide => Value / number.Value,
            OperatorType.GreaterThan => Value > number.Value ? 1 : 0,
            OperatorType.LessThan => Value < number.Value ? 1 : 0,
            OperatorType.Equal => Value == number.Value ? 1 : 0,
            _ => Value,
        };
        OperatorType = OperatorType.None;

        // Destroy other number & reactivate this
        Destroy(number.gameObject);
        Reactivate();

        // Play effects
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Instantiate(splashPrefab, number.transform.position, Quaternion.identity);
        FindFirstObjectByType<Player>().Talk("Math!");

        return true;
    }

    #endregion

    #region Interaction Methods

    private bool Enter(Portal portal)
    {
        if (!portal) return false;
        if (portal.value != Value) return false;

        // Entering portal...
        portal.OnEntered(this);
        TargetPosition = portal.transform.position;
        IsMoving = true;
        Shrink();
        Destroy(gameObject, 0.5f);

        // Play effects
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        GameController.Instance.PlaySlowMotionEffect();

        return true;
    }

    #endregion

    public override bool Move(Vector2 direction)
    {
        if (isStatic) return false;
        if (IsMoving) return false;

        // Raycast to perform operations
        var hit = Physics2D.Raycast(transform.position, direction, 1f);
        if (hit)
        {
            var hitTransform = hit.transform;
            Merge(hitTransform.GetComponent<Operator>());
            Merge(hitTransform.GetComponent<Expression>());
            Operate(hitTransform.GetComponent<Number>());
            if (Enter(hitTransform.GetComponent<Portal>())) return true;
        }

        return base.Move(direction);
    }
}
