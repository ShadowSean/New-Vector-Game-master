using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class RumbleManager : MonoBehaviour
{
    public static RumbleManager Instance { get; private set; }

    private Gamepad _gamepad;
    private Coroutine _rumbleCoroutine;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
        StopRumble();
    }

    private void OnApplicationQuit()
    {
        StopRumble();
    }



    /// <summary>
    /// One-shot pulse — rumbles for <duration> seconds then stops automatically.
    /// </summary>
    /// <param name="lowFreq">Left motor intensity (0–1). Good for heavy impacts.</param>
    /// <param name="highFreq">Right motor intensity (0–1). Good for sharp hits.</param>
    /// <param name="duration">How long to rumble in seconds.</param>
    public void RumblePulse(float lowFreq, float highFreq, float duration)
    {
        StopActiveCoroutine();
        _gamepad = Gamepad.current;

        if (_gamepad == null) return;

        _rumbleCoroutine = StartCoroutine(RumblePulseRoutine(lowFreq, highFreq, duration));
    }


    /// <param name="lowFreq">Left motor intensity (0–1).</param>
    /// <param name="highFreq">Right motor intensity (0–1).</param>
    public void RumbleConstant(float lowFreq, float highFreq)
    {
        StopActiveCoroutine();
        _gamepad = Gamepad.current;

        if (_gamepad == null) return;

        _gamepad.SetMotorSpeeds(lowFreq, highFreq);
    }


    /// <param name="lowFreq">Starting left motor intensity (0–1).</param>
    /// <param name="highFreq">Starting right motor intensity (0–1).</param>
    /// <param name="duration">Fade-out duration in seconds.</param>
    public void RumbleFadeOut(float lowFreq, float highFreq, float duration)
    {
        StopActiveCoroutine();
        _gamepad = Gamepad.current;

        if (_gamepad == null) return;

        _rumbleCoroutine = StartCoroutine(RumbleFadeOutRoutine(lowFreq, highFreq, duration));
    }

    public void StopRumble()
    {
        StopActiveCoroutine();

        _gamepad = Gamepad.current;
        if (_gamepad != null)
        {
            _gamepad.ResetHaptics();
        }
    }



    private IEnumerator RumblePulseRoutine(float lowFreq, float highFreq, float duration)
    {
        _gamepad.SetMotorSpeeds(lowFreq, highFreq);
        yield return new WaitForSecondsRealtime(duration);
        _gamepad.ResetHaptics();
    }

    private IEnumerator RumbleFadeOutRoutine(float lowFreq, float highFreq, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = 1f - Mathf.Clamp01(elapsed / duration);
            _gamepad.SetMotorSpeeds(lowFreq * t, highFreq * t);
            yield return null;
        }

        _gamepad.ResetHaptics();
    }


    private void StopActiveCoroutine()
    {
        if (_rumbleCoroutine != null)
        {
            StopCoroutine(_rumbleCoroutine);
            _rumbleCoroutine = null;
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad && change == InputDeviceChange.Disconnected)
        {
            StopActiveCoroutine();
        }
    }
}