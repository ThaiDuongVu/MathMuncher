using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [Header("Floor References")]
    [SerializeField] private SpriteRenderer mainSprite;

    [Header("Grass References")]
    [SerializeField] private Grass grassPrefab;
    [SerializeField] private int grassCount;
    private List<Vector2> grassPositions = new();

    #region Unity Events

    private void Start()
    {
        GenerateGrass();
    }

    #endregion

    private void GenerateGrass()
    {
        var size = new Vector2Int((int)mainSprite.size.x, (int)mainSprite.size.y);
        var i = 0;
        while (i < grassCount)
        {
            // Generate position based on floor size
            var position = (Vector2)transform.position + new Vector2(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2));
            if (size.x % 2 == 0) position.x += position.x <= 0f ? 0.5f : -0.5f;
            if (size.y % 2 == 0) position.y += position.y <= 0f ? 0.5f : -0.5f;

            // Guard clauses
            if (grassPositions.Contains(position)) continue;
            // Debug.Log(Physics2D.OverlapBoxAll(position, Vector2.one * 0.5f, 0f).Length);
            if (Physics2D.OverlapBoxAll(position, Vector2.one * 0.5f, 0f).Length > 0) continue;

            // Spawn and add grass to list
            var grass = Instantiate(grassPrefab, position, Quaternion.identity);
            grass.transform.SetParent(transform);
            grassPositions.Add(position);
            i++;
        }
    }
}
