using TMPro;
using UnityEngine;

public class Level : MonoBehaviour
{
    #region Singleton

    private static Level _LevelInstance;

    public static Level Instance
    {
        get
        {
            if (_LevelInstance == null) _LevelInstance = FindFirstObjectByType<Level>();
            return _LevelInstance;
        }
    }

    #endregion

    public int levelIndex;

    [Header("Tutorial References")]
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private string mnkTutorial;
    [SerializeField] private string gamepadTutorial;

    #region Unity Events

    private void Update()
    {
        tutorialText.SetText(InputTypeController.Instance.InputType == InputType.MouseKeyboard ? mnkTutorial : gamepadTutorial);
    }

    #endregion
}
