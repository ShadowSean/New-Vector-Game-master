using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class RumbleManager : MonoBehaviour
{
    public static RumbleManager Instance { get; private set; }

    private Gamepad _gamepad;
    private Coroutine _rumbleCoroutine;

    private bool _priorityActive = false;



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

    public void RumblePulse(float lowFreq, float highFreq, float duration)
    {
        StopActiveCoroutine();
        _gamepad = Gamepad.current;
        if (_gamepad == null) return;

        _priorityActive = false;
        _rumbleCoroutine = StartCoroutine(RumblePulseRoutine(lowFreq, highFreq, duration));
    }


    public void RumbleConstant(float lowFreq, float highFreq)
    {
        if (_priorityActive) return;

        _gamepad = Gamepad.current;
        if (_gamepad == null) return;

        _gamepad.SetMotorSpeeds(lowFreq, highFreq);
    }


    public void RumbleFadeOut(float lowFreq, float highFreq, float duration)
    {
        StopActiveCoroutine();
        _gamepad = Gamepad.current;
        if (_gamepad == null) return;

        _priorityActive = false;
        _rumbleCoroutine = StartCoroutine(RumbleFadeOutRoutine(lowFreq, highFreq, duration));
    }


    public void RumbleSequence(float slideLow, float slideHigh, float slideDuration,
                               float thumpLow, float thumpHigh, float thumpDuration)
    {
        StopActiveCoroutine();
        _gamepad = Gamepad.current;
        if (_gamepad == null) return;

        _rumbleCoroutine = StartCoroutine(RumbleSequenceRoutine(
            slideLow, slideHigh, slideDuration,
            thumpLow, thumpHigh, thumpDuration));
    }


    public void StopRumble()
    {
        StopActiveCoroutine();
        _priorityActive = false;

        _gamepad = Gamepad.current;
        if (_gamepad != null)
            _gamepad.ResetHaptics();
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

    private IEnumerator RumbleSequenceRoutine(float slideLow, float slideHigh, float slideDuration,
                                               float thumpLow, float thumpHigh, float thumpDuration)
    {
        _priorityActive = true;

        _gamepad.SetMotorSpeeds(slideLow, slideHigh);
        yield return new WaitForSecondsRealtime(slideDuration);

        _gamepad.SetMotorSpeeds(thumpLow, thumpHigh);
        yield return new WaitForSecondsRealtime(thumpDuration);

        _gamepad.ResetHaptics();
        _priorityActive = false;
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
            _priorityActive = false;
        }
    }
}