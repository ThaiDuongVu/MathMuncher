using System.Linq;
using TMPro;
using UnityEngine;
using System.Collections;

public class CustomizationController : MonoBehaviour
{
    #region Singleton

    private static CustomizationController _customizationControllerInstance;

    public static CustomizationController Instance
    {
        get
        {
            if (_customizationControllerInstance == null) _customizationControllerInstance = FindFirstObjectByType<CustomizationController>();
            return _customizationControllerInstance;
        }
    }

    #endregion

    private int _currentIndex;
    private int CurrentIndex
    {
        get => _currentIndex;
        set
        {
            _currentIndex = value;
            playerAnimator.runtimeAnimatorController = _allSkins[value].frontAnimator;

            // Update texts
            var data = SaveLoadController.Instance.LoadPerma();
            skinText.SetText(_allSkins[value].name);
            costText.SetText(value == data.skinIndex ? "Selected" : $"Requires: {_allSkins[value].cost} stars");
        }
    }

    private const float BufferDuration = 0.1f;
    private bool _isBuffering;

    [Header("Text References")]
    [SerializeField] private TMP_Text starsText;
    [SerializeField] private TMP_Text skinText;
    [SerializeField] private TMP_Text costText;

    [Header("References")]
    [SerializeField] private Animator playerAnimator;

    private Skin[] _allSkins;
    private int _allStars;

    #region Unity Events

    private void Awake()
    {
        _allSkins = Resources.LoadAll<Skin>("Skins");
    }

    private void Start()
    {
        _allStars = GetAllStars();
        starsText.SetText($"Collected: {_allStars} stars");
        var data = SaveLoadController.Instance.LoadPerma();
        CurrentIndex = data.skinIndex;
    }

    #endregion

    #region Getters

    public int GetAllStars()
    {
        var data = SaveLoadController.Instance.LoadPerma();
        var levelRatings = data.levelRatings;
        return levelRatings.Sum();
    }

    #endregion

    public void Select()
    {
        // Check cost before selecting
        var data = SaveLoadController.Instance.LoadPerma();
        if (_allSkins[CurrentIndex].cost > _allStars) return;

        // Select skin index
        data.skinIndex = CurrentIndex;
        SaveLoadController.Instance.SavePerma(data);
        costText.SetText("Selected");
    }

    public void Next()
    {
        if (_isBuffering) return;
        CurrentIndex = CurrentIndex < _allSkins.Length - 1 ? CurrentIndex + 1 : 0;

        // Buffer input
        _isBuffering = true;
        StartCoroutine(DisableBuffer());
    }

    public void Previous()
    {
        if (_isBuffering) return;
        CurrentIndex = CurrentIndex > 0 ? CurrentIndex - 1 : _allSkins.Length - 1;

        // Buffer input
        _isBuffering = true;
        StartCoroutine(DisableBuffer());
    }

    private IEnumerator DisableBuffer()
    {
        yield return new WaitForSecondsRealtime(BufferDuration);
        _isBuffering = false;
    }
}
