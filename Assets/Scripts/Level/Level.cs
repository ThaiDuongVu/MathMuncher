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

    [Header("UI References")]
    [SerializeField] private Canvas hud;
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private string mnkText;
    [SerializeField] private string gamepadText;

    #region Unity Events

    private void Update()
    {
        hud.gameObject.SetActive(GameController.Instance.State == GameState.InProgress);
        displayText.SetText(InputTypeController.Instance.InputType == InputType.MouseKeyboard ? mnkText : gamepadText);
    }

    #endregion
}
