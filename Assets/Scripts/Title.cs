using UnityEngine;

public class Title : MonoBehaviour
{
    [Header("Block Prefabs")]
    [SerializeField] private Block blockPrefab;
    [SerializeField] private Vector2[] blockSpawnPositions;

    // Spawn random numbers when title animation is completed
    public void SpawnBlocks()
    {
        for (int i = 0; i < Random.Range(2, 5); i++)
        {
            // Choose spawn position
            var index = Random.Range(0, blockSpawnPositions.Length);
            while (blockSpawnPositions[index] == Vector2.zero)
                index = Random.Range(0, blockSpawnPositions.Length);
            var position = blockSpawnPositions[index];

            // Spawn & update array
            var block = Instantiate(blockPrefab, position, Quaternion.identity);
            block.SetText(Random.Range(1, 10).ToString());
            block.transform.localScale = Vector2.zero;
            blockSpawnPositions[index] = Vector2.zero;
        }
    }
}
