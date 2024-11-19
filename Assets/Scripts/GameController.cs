using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    #region Singleton

    private static GameController _gameControllerInstance;

    public static GameController Instance
    {
        get
        {
            if (_gameControllerInstance == null) _gameControllerInstance = FindFirstObjectByType<GameController>();
            return _gameControllerInstance;
        }
    }

    #endregion

    public GameState State { get; private set; }
    public bool IsInProgress => State == GameState.InProgress;

    [Header("Menu References")]
    [SerializeField] private SimpleMenu pauseMenu;
    [SerializeField] private SimpleMenu gameOverMenu;
    [SerializeField] private SimpleMenu levelCompleteMenu;

    [Header("Audio References")]
    [SerializeField] private AudioSource gameOverAudio;
    [SerializeField] private AudioSource levelCompleteAudio;

    private InputActions _inputActions;

    #region Unity Events

    private void OnEnable()
    {
        _inputActions = new InputActions();

        // Handle player pause/resume input
        _inputActions.Game.Start.performed += StartOnPerformed;
        // Handle select input
        _inputActions.Game.Select.performed += SelectOnPerformed;

        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void Start()
    {
        SetTimeScale();
    }

    private void Update()
    {
        // Check level complete
        var blocksCount = FindObjectsByType<Block>(FindObjectsSortMode.None).Length;
        var operatorCount = FindObjectsByType<Operator>(FindObjectsSortMode.None).Length;
        if (Level.Instance.levelIndex != -1 && blocksCount == 0 && operatorCount == 0)
            CompleteLevel(3 - FindObjectsByType<Star>(FindObjectsSortMode.None).Length);
    }

    #endregion

    #region Input Handlers

    private void StartOnPerformed(InputAction.CallbackContext context)
    {
        if (State == GameState.InProgress) Pause();
        // else if (State == GameState.Paused) Resume();
    }

    private void SelectOnPerformed(InputAction.CallbackContext context)
    {
        SceneLoader.Instance.Restart();
    }

    #endregion

    #region Pause/Resume Methods

    private void Pause()
    {
        SetTimeScale(0f);
        pauseMenu.SetActive(true);

        SetGameState(GameState.Paused);
        PostProcessingController.Instance.SetDepthOfField(true);
    }

    public void Resume()
    {
        SetTimeScale();
        pauseMenu.SetActive(false);

        SetGameState(GameState.InProgress);
        PostProcessingController.Instance.SetDepthOfField(false);
    }

    #endregion

    #region Game Over & Level Complete Methods

    public IEnumerator GameOver()
    {
        if (State != GameState.InProgress) yield break;
        SetGameState(GameState.Over);
        yield return new WaitForSeconds(0.5f);

        // Play effects
        gameOverMenu.SetActive(true);
        PostProcessingController.Instance.SetDepthOfField(true);
        gameOverAudio.Play();
    }

    private void CompleteLevel(int stars)
    {
        if (State != GameState.InProgress) return;
        SetGameState(GameState.Over);

        // Play effects
        levelCompleteMenu.GetComponentInChildren<StarsDisplay>().SetStars(stars);
        levelCompleteMenu.SetActive(true);
        PostProcessingController.Instance.SetDepthOfField(true);
        levelCompleteAudio.Play();

        // Update save
        UpdateLevelSave(Level.Instance.levelIndex, stars);
    }

    #endregion

    private void SetGameState(GameState state)
    {
        State = state;
    }

    public static void SetTimeScale(float scale = 1f)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.01666667f * Time.timeScale;
    }

    private void UpdateLevelSave(int levelIndex, int stars)
    {
        var permaSaveData = SaveLoadController.Instance.LoadPerma();

        // If saved index is less than current index then add to it
        if (permaSaveData.unlockedLevelIndex <= levelIndex)
            permaSaveData.unlockedLevelIndex = levelIndex + 1;

        // If saved stars is less than collected stars then set new stars
        if (permaSaveData.levelRatings.Count < levelIndex)
            permaSaveData.levelRatings.Add(stars);
        else
        {
            if (permaSaveData.levelRatings[levelIndex - 1] < stars)
                permaSaveData.levelRatings[levelIndex - 1] = stars;
        }

        SaveLoadController.Instance.SavePerma(permaSaveData);
    }

    #region Slow Motion Methods

    private static IEnumerator SlowMotionEffect(float scale, float duration)
    {
        if (Time.timeScale < 1f) yield break;

        // Slow down
        SetTimeScale(scale);
        PostProcessingController.Instance.SetChromaticAberration(true);
        PostProcessingController.Instance.SetVignetteIntensity(PostProcessingController.DefaultVignetteIntensity + 0.05f);

        yield return new WaitForSecondsRealtime(duration);

        // Back to normal
        SetTimeScale();
        PostProcessingController.Instance.SetChromaticAberration(false);
        PostProcessingController.Instance.SetVignetteIntensity(PostProcessingController.DefaultVignetteIntensity);
    }

    public void PlaySlowMotionEffect(float scale = 0.5f, float duration = 0.2f)
    {
        StartCoroutine(SlowMotionEffect(scale, duration));
    }

    #endregion
}
