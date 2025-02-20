using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class GamepadRumbler : MonoBehaviour
{
    #region Singleton

    private static GamepadRumbler _gamepadRumblerInstance;

    public static GamepadRumbler Instance
    {
        get
        {
            if (_gamepadRumblerInstance == null) _gamepadRumblerInstance = FindFirstObjectByType<GamepadRumbler>();
            return _gamepadRumblerInstance;
        }
    }

    #endregion

    public bool IsEnabled { get; set; }

    private static IEnumerator StartRumble(float duration, float intensity)
    {
        Gamepad.current.SetMotorSpeeds(intensity, intensity);
        yield return new WaitForSeconds(duration);
        InputSystem.ResetHaptics();
    }

    #region Rumble Methods

    public void Rumble(GamepadRumbleMode gamepadRumbleMode)
    {
        // If no gamepad connected or vibration disabled then return
        if (Gamepad.current == null || !IsEnabled) return;

        StopAllCoroutines();
        switch (gamepadRumbleMode)
        {
            case GamepadRumbleMode.Nano:
                StartCoroutine(StartRumble(0.02f, 0.02f));
                break;

            case GamepadRumbleMode.Micro:
                StartCoroutine(StartRumble(0.06f, 0.06f));
                break;

            case GamepadRumbleMode.Light:
                StartCoroutine(StartRumble(0.08f, 0.08f));
                break;

            case GamepadRumbleMode.Normal:
                StartCoroutine(StartRumble(0.1f, 0.1f));
                break;

            case GamepadRumbleMode.Hard:
                StartCoroutine(StartRumble(0.15f, 0.15f));
                break;

            default:
                return;
        }
    }

    public void Rumble(float leftHaptic, float rightHaptic)
    {
        // If no gamepad connected or vibration disabled then return
        if (Gamepad.current == null || !IsEnabled) return;

        StopAllCoroutines();
        StartCoroutine(StartRumble(leftHaptic, rightHaptic));
    }

    #endregion
}