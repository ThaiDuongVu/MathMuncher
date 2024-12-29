using System.Collections;
using UnityEngine;

public class TurnPlatform : Platform, ITurnable
{
    [Header("Position References")]
    [SerializeField]
    private Vector2[] positions;
    [SerializeField] private LineRenderer positionLine;
    private int _positionIndex;

    #region Unity Events

    protected override void Start()
    {
        base.Start();

        positionLine.positionCount = positions.Length;
        for (int i = 0; i < positions.Length; i++)
        {
            var position = positions[i];
            positionLine.SetPosition(i, position);
        }
    }

    #endregion

    #region Interface Implementations

    public void UpdateTurn(Vector2 direction)
    {
        // Update index
        if (_positionIndex < positions.Length - 1) _positionIndex++;
        else _positionIndex = 0;

        // Move to new position
        var newDirection = (positions[_positionIndex] - (Vector2)transform.position).normalized;
        StartCoroutine(ForceMove(newDirection));
    }

    #endregion

    public IEnumerator ForceMove(Vector2 direction)
    {
        yield return new WaitForEndOfFrame();
        // Cast to check if movable
        // var hit = Physics2D.Raycast(transform.position, direction, RaycastDistance);
        var hits = Physics2D.BoxCastAll(transform.position, CastSize, 0f, direction, CastDistance);
        foreach (var hit in hits)
        {
            var actor = hit.transform.GetComponent<Actor>();
            if (!actor) yield break;
            if (!actor.Move(direction)) yield break;
        }

        TargetPosition += direction;
        IsMoving = true;
    }
}
