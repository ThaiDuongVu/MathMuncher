using TMPro;
using UnityEngine;

public class Level : MonoBehaviour
{
    #region Singleton

    private static Level _levelInstance;

    public static Level Instance
    {
        get
        {
            if (_levelInstance == null) _levelInstance = FindFirstObjectByType<Level>();
            return _levelInstance;
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
