using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleManager : MonoBehaviour
{
    public static RumbleManager Instance { get; private set; }


    [Header("Accessibility")]
    public bool hapticsEnabled = true;
    [Range(0f, 1f)] public float masterIntensity = 1f;


    private Gamepad _gamepad;
    private Coroutine _rumbleCoroutine;
    private bool _priorityActive = false;


    private void Awake()
    {
        //if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() => InputSystem.onDeviceChange += OnDeviceChange;
    private void OnDisable() { InputSystem.onDeviceChange -= OnDeviceChange; StopRumble(); }
    private void OnApplicationQuit() => StopRumble();


    public void RumblePulse(float lowFreq, float highFreq, float duration)
    {
        if (!hapticsEnabled) return;
        if (_priorityActive) return;
        StopActiveCoroutine();
        _gamepad = Gamepad.current;
        if (_gamepad == null) return;

        _priorityActive = false;
        _rumbleCoroutine = StartCoroutine(RumblePulseRoutine(
            lowFreq * masterIntensity,
            highFreq * masterIntensity,
            duration));
    }

    public void RumbleConstant(float lowFreq, float highFreq)
    {
        if (!hapticsEnabled || _priorityActive) return;
        _gamepad = Gamepad.current;
        if (_gamepad == null) return;
        _gamepad.SetMotorSpeeds(
            lowFreq * masterIntensity,
            highFreq * masterIntensity);
    }

    public void RumbleFadeOut(float lowFreq, float highFreq, float duration)
    {
        if (!hapticsEnabled) return;
        StopActiveCoroutine();
        _gamepad = Gamepad.current;
        if (_gamepad == null) return;

        _priorityActive = false;
        _rumbleCoroutine = StartCoroutine(RumbleFadeOutRoutine(
            lowFreq * masterIntensity,
            highFreq * masterIntensity,
            duration));
    }

    public void RumbleSequence(float slideLow, float slideHigh, float slideDuration,
                               float thumpLow, float thumpHigh, float thumpDuration)
    {
        if (!hapticsEnabled) return;
        StopActiveCoroutine();
        _gamepad = Gamepad.current;
        if (_gamepad == null) return;

        _rumbleCoroutine = StartCoroutine(RumbleSequenceRoutine(
            slideLow * masterIntensity, slideHigh * masterIntensity, slideDuration,
            thumpLow * masterIntensity, thumpHigh * masterIntensity, thumpDuration));
    }

    public void Play(RumbleProfile profile, bool priority = false)
    {
        if (!hapticsEnabled || profile == null) return;
        if (_priorityActive && !priority) return;

        StopActiveCoroutine();
        _gamepad = Gamepad.current;
        if (_gamepad == null) return;

        _priorityActive = priority;
        _rumbleCoroutine = StartCoroutine(RunProfile(profile));
    }


    public void StopAll() => StopRumble();


    public void StopRumble()
    {
        StopActiveCoroutine();
        _priorityActive = false;
        _gamepad = Gamepad.current;
        if (_gamepad != null) _gamepad.ResetHaptics();
    }
    private IEnumerator RumblePulseRoutine(float low, float high, float duration)
    {
        _gamepad.SetMotorSpeeds(low, high);
        yield return new WaitForSecondsRealtime(duration);
        _gamepad.ResetHaptics();
    }

    private IEnumerator RumbleFadeOutRoutine(float low, float high, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = 1f - Mathf.Clamp01(elapsed / duration);
            _gamepad.SetMotorSpeeds(low * t, high * t);
            yield return null;
        }
        _gamepad.ResetHaptics();
    }

    private IEnumerator RumbleSequenceRoutine(float slideLow, float slideHigh, float slideDur,
                                              float thumpLow, float thumpHigh, float thumpDur)
    {
        _priorityActive = true;

        _gamepad.SetMotorSpeeds(slideLow, slideHigh);
        yield return new WaitForSecondsRealtime(slideDur);

        _gamepad.SetMotorSpeeds(thumpLow, thumpHigh);
        yield return new WaitForSecondsRealtime(thumpDur);

        _gamepad.ResetHaptics();
        _priorityActive = false;
    }


    private IEnumerator RunProfile(RumbleProfile profile)
    {
        foreach (var phase in profile.phases)
        {
            float elapsed = 0f;
            while (elapsed < phase.duration)
            {
                float n = elapsed / phase.duration;
                _gamepad.SetMotorSpeeds(
                    Mathf.Clamp01(phase.lowMotor.Evaluate(n) * masterIntensity),
                    Mathf.Clamp01(phase.highMotor.Evaluate(n) * masterIntensity));
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
        }
        _gamepad.ResetHaptics();
        _priorityActive = false;
        _rumbleCoroutine = null;
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