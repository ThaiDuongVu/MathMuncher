using UnityEngine;
using TMPro;

public class Enemy : Character, ITurnable
{
    [SerializeField] private AudioSource explosionAudio;

    [Header("Enemy References")]
    [SerializeField] private RectTransform overlay;
    [SerializeField] private TMP_Text text;

    [SerializeField] private ParticleSystem splashPrefab;
    [SerializeField] private Vector2[] positions;

    private int _positionIndex;

    public Condition Condition { get; private set; }
    private Camera _mainCamera;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _mainCamera = Camera.main;
        Condition = GetComponent<Condition>();
    }

    protected override void Start()
    {
        base.Start();

        SetText(Condition.ToString());
    }

    protected override void Update()
    {
        base.Update();

        if (GameController.Instance)
            overlay.gameObject.SetActive(GameController.Instance.State == GameState.InProgress);
        else if (HomeController.Instance)
            overlay.gameObject.SetActive(!HomeController.Instance.IsInit);

        overlay.position = _mainCamera.WorldToScreenPoint(transform.position);
    }

    #endregion

    #region Interface Implementations

    public void NextTurn()
    {
        if (positions.Length == 0) return;

        // Update index & direction
        _positionIndex = _positionIndex >= positions.Length - 1 ? 0 : _positionIndex + 1;
        var direction = (positions[_positionIndex] - (Vector2)transform.position).normalized;
        ForceMove(direction);
    }

    #endregion

    public virtual void SetText(string message)
    {
        text.SetText(message);
    }

    public void DestroyObject(GameObject other)
    {
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Instantiate(splashPrefab, other.transform.position, Quaternion.identity);
        GameController.Instance.PlaySlowMotionEffect();
        explosionAudio.Play();

        Destroy(other);
    }

    public override bool Move(Vector2 direction)
    {
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var number = other.GetComponent<Number>();
        if (!number)
        {
            DestroyObject(other.gameObject);
            return;
        }

        if (Condition.Evaluate(number.Value)) DestroyObject(gameObject);
        else DestroyObject(other.gameObject);
    }
}
