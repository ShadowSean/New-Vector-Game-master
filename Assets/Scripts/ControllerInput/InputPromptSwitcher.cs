using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InputPromptSwitcher : MonoBehaviour
{
    [Header("Text Target")]
    [SerializeField] private TMP_Text promptText;

    [Header("Gamepad Prompts")]
    [SerializeField] private List<InputPrompt> gamepadPrompts = new List<InputPrompt>();

    [Header("Keyboard & Mouse Prompts")]
    [SerializeField] private List<InputPrompt> keyboardPrompts = new List<InputPrompt>();

    [Header("Settings")]
    [SerializeField] private string defaultAction = "interact";

    [Tooltip("Seconds before the device type can switch again. Prevents flashing.")]
    [SerializeField] private float switchCooldown = 0.5f;

    [Tooltip("Stick/trigger must exceed this to count as gamepad input.")]
    [SerializeField] private float gamepadDeadzone = 0.2f;

    private bool usingGamepad = false;
    private string currentAction;
    private float lastSwitchTime = -999f;


    void Reset()
    {

        gamepadPrompts = new List<InputPrompt>
        {
            new InputPrompt { action = "interact", text = "Press (A) to interact" },
            new InputPrompt { action = "jump",     text = "Press (B) to jump"     },
        };
        keyboardPrompts = new List<InputPrompt>
        {
            new InputPrompt { action = "interact", text = "Press [E] to interact" },
            new InputPrompt { action = "jump",     text = "Press [Space] to jump" },
        };
        defaultAction = "interact";
    }

    void Start()
    {
        if (promptText == null)
        {
            Debug.LogError("[InputPromptSwitcher] No TMP_Text assigned! Drag your text object into the Prompt Text slot.", this);
            return;
        }

        if (gamepadPrompts.Count == 0)
            Debug.LogWarning("[InputPromptSwitcher] Gamepad Prompts list is empty.", this);

        if (keyboardPrompts.Count == 0)
            Debug.LogWarning("[InputPromptSwitcher] Keyboard & Mouse Prompts list is empty.", this);

        currentAction = defaultAction;
        usingGamepad = Gamepad.current != null;
        RefreshText();
    }

    void Update()
    {
        if (promptText == null) return;
        if (Time.unscaledTime - lastSwitchTime < switchCooldown) return;

        if (!usingGamepad && IsGamepadActive())
        {
            usingGamepad = true;
            lastSwitchTime = Time.unscaledTime;
            RefreshText();
        }
        else if (usingGamepad && IsKeyboardMouseActive())
        {
            usingGamepad = false;
            lastSwitchTime = Time.unscaledTime;
            RefreshText();
        }
    }


    public void SetAction(string action)
    {
        currentAction = action;
        RefreshText();
    }


    private bool IsGamepadActive()
    {
        var gp = Gamepad.current;
        if (gp == null) return false;

        foreach (var control in gp.allControls)
        {
            if (control is UnityEngine.InputSystem.Controls.ButtonControl btn && btn.wasPressedThisFrame)
                return true;
        }

        if (gp.leftStick.ReadValue().magnitude > gamepadDeadzone) return true;
        if (gp.rightStick.ReadValue().magnitude > gamepadDeadzone) return true;
        if (gp.leftTrigger.ReadValue() > gamepadDeadzone) return true;
        if (gp.rightTrigger.ReadValue() > gamepadDeadzone) return true;

        return false;
    }

    private bool IsKeyboardMouseActive()
    {
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            return true;

        if (Mouse.current != null)
        {
            if (Mouse.current.delta.ReadValue().magnitude > 0.1f) return true;
            if (Mouse.current.leftButton.wasPressedThisFrame) return true;
            if (Mouse.current.rightButton.wasPressedThisFrame) return true;
        }

        return false;
    }

    private void RefreshText()
    {
        if (promptText == null) return;

        List<InputPrompt> list = usingGamepad ? gamepadPrompts : keyboardPrompts;
        string deviceName = usingGamepad ? "Gamepad" : "Keyboard & Mouse";

        foreach (InputPrompt prompt in list)
        {
            if (prompt.action == currentAction)
            {
                promptText.text = prompt.text;
                return;
            }
        }

        if (list.Count > 0)
        {
            promptText.text = list[0].text;
            Debug.LogWarning($"[InputPromptSwitcher] No '{currentAction}' entry found in {deviceName} list. Showing first entry as fallback.", this);
        }
        else
        {
            Debug.LogError($"[InputPromptSwitcher] {deviceName} list is empty — nothing to display.", this);
        }
    }


    [System.Serializable]
    public class InputPrompt
    {
        public string action;
        [TextArea(1, 2)]
        public string text;
    }
}