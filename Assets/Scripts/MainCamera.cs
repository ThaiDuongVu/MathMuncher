using System.Collections;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Vector3 TargetPosition { get; set; }
    public Vector3 OffsetPosition { get; private set; }
    private const float PositionLerpSpeed = 10f;

    private Animator _animator;
    private static readonly int IntroAnimationTrigger = Animator.StringToHash("intro");
    private static readonly int OutroAnimationTrigger = Animator.StringToHash("outro");
    private static readonly int ZoomAnimationBool = Animator.StringToHash("isZoomed");

    #region Unity Events

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        TargetPosition = transform.position;
        OffsetPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, PositionLerpSpeed * Time.deltaTime);
    }

    #endregion

    #region Intro & Outro

    public void Intro()
    {
        _animator.SetTrigger(IntroAnimationTrigger);
    }

    public void Outro()
    {
        _animator.SetTrigger(OutroAnimationTrigger);
    }

    public void SetZoomed(bool isZoomed)
    {
        _animator.SetBool(ZoomAnimationBool, isZoomed);
    }

    #endregion

    public IEnumerator TempZoom(Vector3 position, float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        // Zoom in
        SetZoomed(true);
        TargetPosition = position + OffsetPosition;

        yield return new WaitForSeconds(duration);

        // Zoom out
        SetZoomed(false);
        TargetPosition = OffsetPosition;
    }
}
