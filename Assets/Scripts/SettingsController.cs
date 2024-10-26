using System.Collections;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private SimpleButton fullscreenButton;
    [SerializeField] private SimpleButton resolutionButton;
    [SerializeField] private SimpleButton cameraShakeButton;
    [SerializeField] private SimpleButton gamepadVibrationButton;
    [SerializeField] private SimpleButton audioButton;

    private const float BufferDuration = 0.1f;
    private bool _isBuffering;

    #region Unity Events

    private void Start()
    {
        // Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;

        Apply();
    }

    #endregion

    #region Toggle Methods

    public void ToggleFullscreen()
    {
        if (_isBuffering) return;

        // Toggle settings
        var settingsSaveData = SaveLoadController.Instance.LoadSettings();
        settingsSaveData.fullscreen = settingsSaveData.fullscreen == 0 ? 1 : 0;
        SaveLoadController.Instance.SaveSettings(settingsSaveData);

        // Apply settings
        Apply();

        _isBuffering = true;
        StartCoroutine(DisableBuffer());
    }

    public void ToggleResolution()
    {
        if (_isBuffering) return;

        // Toggle settings
        var settingsSaveData = SaveLoadController.Instance.LoadSettings();
        if (settingsSaveData.resolution < 5) settingsSaveData.resolution++;
        else settingsSaveData.resolution = 0;
        SaveLoadController.Instance.SaveSettings(settingsSaveData);

        // Apply settings
        Apply();

        _isBuffering = true;
        StartCoroutine(DisableBuffer());
    }

    public void ToggleCameraShake()
    {
        if (_isBuffering) return;

        // Toggle settings
        var settingsSaveData = SaveLoadController.Instance.LoadSettings();
        settingsSaveData.cameraShake = !settingsSaveData.cameraShake;
        SaveLoadController.Instance.SaveSettings(settingsSaveData);

        // Apply settings
        Apply();

        _isBuffering = true;
        StartCoroutine(DisableBuffer());
    }

    public void ToggleGamepadVibration()
    {
        if (_isBuffering) return;

        // Toggle settings
        var settingsSaveData = SaveLoadController.Instance.LoadSettings();
        settingsSaveData.gamepadVibration = !settingsSaveData.gamepadVibration;
        SaveLoadController.Instance.SaveSettings(settingsSaveData);

        // Apply settings
        Apply();

        _isBuffering = true;
        StartCoroutine(DisableBuffer());
    }

    public void ToggleAudio()
    {
        if (_isBuffering) return;

        // Toggle settings
        var settingsSaveData = SaveLoadController.Instance.LoadSettings();
        settingsSaveData.audio = !settingsSaveData.audio;
        SaveLoadController.Instance.SaveSettings(settingsSaveData);

        // Apply settings
        Apply();

        _isBuffering = true;
        StartCoroutine(DisableBuffer());
    }

    #endregion

    private IEnumerator DisableBuffer()
    {
        yield return new WaitForSecondsRealtime(BufferDuration);
        _isBuffering = false;
    }

    private void Apply()
    {
        var settingsSaveData = SaveLoadController.Instance.LoadSettings();

        // Apply fullscreen setting
        // 0: fullscreen
        // 1: windowed
        var fullscreenValue = settingsSaveData.fullscreen;
        fullscreenButton.SetMainText(fullscreenValue == 0 ? "Fullscreen" : "Windowed");

        // Apply resolution setting
        // 0: 640x360
        // 1: 1280x720
        // 2: 1600x900
        // 3: 1920x1080
        // 4: 2560x1440
        // 5: 3840x2160
        var resolutionValue = settingsSaveData.resolution;
        switch (resolutionValue)
        {
            case 0:
                Screen.SetResolution(640, 360, fullscreenValue == 0);
                resolutionButton.SetMainText("640 x 360");
                break;

            case 1:
                Screen.SetResolution(1280, 720, fullscreenValue == 0);
                resolutionButton.SetMainText("1280 x 720");
                break;

            case 2:
                Screen.SetResolution(1600, 900, fullscreenValue == 0);
                resolutionButton.SetMainText("1600 x 900");
                break;

            case 3:
                Screen.SetResolution(1920, 1080, fullscreenValue == 0);
                resolutionButton.SetMainText("1920 x 1080");
                break;

            case 4:
                Screen.SetResolution(2560, 1440, fullscreenValue == 0);
                resolutionButton.SetMainText("2560 x 1440");
                break;

            case 5:
                Screen.SetResolution(3840, 2160, fullscreenValue == 0);
                resolutionButton.SetMainText("3840 x 2160");
                break;
        }

        // Apply camera shake setting
        var cameraShakeValue = settingsSaveData.cameraShake;
        cameraShakeButton.SetMainText(cameraShakeValue ? "On" : "Off");
        CameraShaker.Instance.IsEnabled = cameraShakeValue;

        // Apply gamepad vibration setting
        var gamepadVibrationValue = settingsSaveData.gamepadVibration;
        gamepadVibrationButton.SetMainText(gamepadVibrationValue ? "On" : "Off");
        GamepadRumbler.Instance.IsEnabled = gamepadVibrationValue;

        // Apply audio settings
        var audioValue = settingsSaveData.audio;
        audioButton.SetMainText(audioValue ? "On" : "Off");
        AudioListener.volume = audioValue ? 1f : 0f;
    }
}
