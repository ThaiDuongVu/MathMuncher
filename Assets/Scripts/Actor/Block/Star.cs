using UnityEngine;

public class Star : Block
{
    [Header("Star References")]
    [SerializeField] private ParticleSystem starSplashPrefab;

    public void OnCollected(Block block)
    {
        Instantiate(starSplashPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
