using System.Linq;
using TMPro;
using UnityEngine;

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
            Select(value);
        }
    }

    [SerializeField] private TMP_Text starsText;
    [SerializeField] private TMP_Text skinText;
    [SerializeField] private TMP_Text costText;

    #region Unity Events

    private void Start()
    {
        starsText.SetText($"Collected: {GetAllStars()} stars");
    }

    #endregion

    #region Getters

    public int GetAllStars()
    {
        var permaSaveData = SaveLoadController.Instance.LoadPerma();
        var levelRatings = permaSaveData.levelRatings;
        return levelRatings.Sum();
    }

    #endregion

    private void Select(int skinIndex)
    {
        
    }
}
