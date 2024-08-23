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

    private const float BufferDuration = 0.1f;
    private bool _isBuffering;

    private int _currentIndex;
    public int CurrentIndex
    {
        get => _currentIndex;
        set
        {
            if (_isBuffering) return;

            _currentIndex = value;
            Select(value);

            _isBuffering = true;
            Invoke(nameof(DisableBuffer), BufferDuration);
        }
    }

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

    private void DisableBuffer()
    {
        _isBuffering = false;
    }

    public bool LevelUnlocked(int levelIndex)
    {
        var permaSaveData = SaveLoadController.Instance.LoadPerma();
        return permaSaveData.unlockedLevelIndex >= levelIndex;
    }

    public int LevelRating(int levelIndex)
    {
        var permaSaveData = SaveLoadController.Instance.LoadPerma();
        if (permaSaveData.levelRatings.Count > levelIndex - 1)
            return permaSaveData.levelRatings[levelIndex - 1];

        return 0;
    }

    public void Select(int levelIndex)
    {
        levelThumbnail.sprite = _allThumbnails[levelIndex - 1];
        levelText.SetText($"Level {levelIndex:D2}{(LevelUnlocked(levelIndex) ? "" : " - Locked")}");
        starsDisplay.SetStars(LevelRating(levelIndex));
    }

    #region Menu Methods

    public void LoadSelected()
    {
        if (!LevelUnlocked(CurrentIndex)) return;
        SceneLoader.Instance.Load($"Level_{CurrentIndex:D2}");
    }

    public void Next()
    {
        CurrentIndex = CurrentIndex < _allThumbnails.Length ? CurrentIndex + 1 : 1;
    }

    public void Previous()
    {
        CurrentIndex = CurrentIndex > 1 ? CurrentIndex - 1 : _allThumbnails.Length;
    }

    #endregion
}
