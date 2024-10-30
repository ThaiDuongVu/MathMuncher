using UnityEngine;
using TMPro;

public class Operator : Actor
{
    [Header("Operator References")]
    [SerializeField] private OperatorType type;
    [SerializeField] private int value;
    [SerializeField] private TMP_Text operationText;

    [Header("Effects References")]
    [SerializeField] private ParticleSystem splashPrefab;

    #region Unity Events

    protected override void Start()
    {
        base.Start();

        operationText.SetText($"{OperatorSymbol(type)}{((value == -1000) ? "" : value)}");
    }

    #endregion

    public int Operate(int operateValue)
    {
        return type switch
        {
            OperatorType.None => operateValue,
            OperatorType.Addition => operateValue + value,
            OperatorType.Subtraction => operateValue - value,
            OperatorType.Multiplication => operateValue * value,
            OperatorType.Division => operateValue / value,
            OperatorType.Square => operateValue * operateValue,
            OperatorType.SquareRoot => (int)Mathf.Sqrt(operateValue),
            OperatorType.Factorial => Factorial(operateValue),
            OperatorType.Greater => operateValue > value ? 1 : 0,
            OperatorType.Less => operateValue < value ? 1 : 0,
            OperatorType.Equal => operateValue == value ? 1 : 0,
            _ => operateValue,
        };
    }

    private static string OperatorSymbol(OperatorType type)
    {
        return type switch
        {
            OperatorType.None => "",
            OperatorType.Addition => "+",
            OperatorType.Subtraction => "-",
            OperatorType.Multiplication => "*",
            OperatorType.Division => "/",
            OperatorType.Square => "**2",
            OperatorType.SquareRoot => "SQ\nRT",
            OperatorType.Factorial => "!",
            OperatorType.Greater => ">",
            OperatorType.Less => "<",
            OperatorType.Equal => "=",
            _ => "",
        };
    }

    private static int Factorial(int value)
    {
        var result = 1;
        for (var i = 1; i <= value; i++)
            result *= i;

        return result;
    }

    public override void Explode()
    {
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        GameController.Instance.PlaySlowMotionEffect();
        Instantiate(splashPrefab, transform.position, Quaternion.identity);

        base.Explode();
    }

    protected override bool Move(Vector2 direction)
    {
        if (isStatic) return false;

        // Raycast
        var hit = Physics2D.Raycast(transform.position, direction, 1f);
        if (hit)
        {
            // Merge
            var block = hit.transform.GetComponent<Block>();
            if (block) return block.Merge(this);

            // Interact
            var interactable = hit.transform.GetComponent<Interactable>();
            if (interactable && interactable.OnInteracted(this)) return true;
        }

        return base.Move(direction);
    }
}