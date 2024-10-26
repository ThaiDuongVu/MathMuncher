using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    private Sprite[] _allThumbnails;

    [Header("UI References")]
    [SerializeField] private Image levelThumbnail;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private StarsDisplay starsDisplay;

    private int _currentIndex;
    private int CurrentIndex
    {
        get => _currentIndex;
        set
        {
            _currentIndex = value;
            Select(value);
        }
    }

    private const float BufferDuration = 0.1f;
    private bool _isBuffering;

    #region Unity Events

    private void Awake()
    {
        _allThumbnails = Resources.LoadAll<Sprite>("LevelThumbnails");
    }

    private void Start()
    {
        CurrentIndex = 1;
    }

    #endregion

    #region Getters

    private bool GetLevelUnlocked(int levelIndex)
    {
        var permaSaveData = SaveLoadController.Instance.LoadPerma();
        return permaSaveData.unlockedLevelIndex >= levelIndex;
    }

    private int GetLevelRating(int levelIndex)
    {
        var permaSaveData = SaveLoadController.Instance.LoadPerma();
        return permaSaveData.levelRatings.Count > levelIndex - 1 ? permaSaveData.levelRatings[levelIndex - 1] : 0;
    }

    #endregion

    private void Select(int levelIndex)
    {
        levelThumbnail.sprite = _allThumbnails[levelIndex - 1];
        levelText.SetText($"Level {levelIndex:D2}{(GetLevelUnlocked(levelIndex) ? "" : " - Locked")}");
        starsDisplay.SetStars(GetLevelRating(levelIndex));
    }

    #region Menu Methods

    public void LoadSelected()
    {
        if (!GetLevelUnlocked(CurrentIndex)) return;
        SceneLoader.Instance.Load($"Level_{CurrentIndex:D2}");
    }

    public void Next()
    {
        if (_isBuffering) return;
        CurrentIndex = CurrentIndex < _allThumbnails.Length ? CurrentIndex + 1 : 1;

        _isBuffering = true;
        StartCoroutine(DisableBuffer());
    }

    public void Previous()
    {
        if (_isBuffering) return;
        CurrentIndex = CurrentIndex > 1 ? CurrentIndex - 1 : _allThumbnails.Length;

        _isBuffering = true;
        StartCoroutine(DisableBuffer());
    }

    #endregion

    private IEnumerator DisableBuffer()
    {
        yield return new WaitForSecondsRealtime(BufferDuration);
        _isBuffering = false;
    }
}
