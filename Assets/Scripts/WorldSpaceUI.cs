using UnityEngine;

public class WorldSpaceUI : MonoBehaviour
{
    [SerializeField] private Canvas overlay;
    private RectTransform[] _overlayElements;
    private Camera _mainCamera;

    #region Unity Events

    private void Awake()
    {
        _overlayElements = overlay.GetComponentsInChildren<RectTransform>();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (GameController.Instance)
            overlay.gameObject.SetActive(GameController.Instance.State == GameState.InProgress);
        if (HomeController.Instance)
            overlay.gameObject.SetActive(!HomeController.Instance.IsInit);

        for (int i = 1; i < _overlayElements.Length; i++)
        {
            _overlayElements[i].position = _mainCamera.WorldToScreenPoint(transform.position);
            _overlayElements[i].localScale = transform.lossyScale;
        }
    }

    #endregion
}
