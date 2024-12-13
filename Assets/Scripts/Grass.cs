using System.Collections;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [Header("Grass References")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite[] sprites;

    private Animator _animator;
    private static readonly int SwayAnimationTrigger = Animator.StringToHash("sway");

    #region Unity Events

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        sprite.sprite = sprites[Random.Range(0, sprites.Length)];
        StartCoroutine(Sway());
    }

    #endregion

    private IEnumerator Sway()
    {
        _animator.SetTrigger(SwayAnimationTrigger);
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        StartCoroutine(Sway());
    }
}
