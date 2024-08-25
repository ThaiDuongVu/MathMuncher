using UnityEngine;
using TMPro;

public class Enemy : Character
{
    [Header("Enemy References")]
    [SerializeField] private RectTransform overlay;
    [SerializeField] private TMP_Text text;

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

    public virtual void SetText(string message)
    {
        text.SetText(message);
    }
}
