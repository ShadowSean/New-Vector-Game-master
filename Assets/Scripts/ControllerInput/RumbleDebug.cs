using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleDebug : MonoBehaviour
{
    void Update()
    {
        var gp = Gamepad.current;

        if (gp == null)
        {
            Debug.Log("No gamepad detected");
            return;
        }

        Debug.Log($"Gamepad: {gp.displayName}");

        // Hold any face button to trigger rumble
        if (gp.buttonSouth.wasPressedThisFrame)
        {
            Debug.Log("Sending rumble...");
            gp.SetMotorSpeeds(1f, 1f);
        }

        if (gp.buttonNorth.wasPressedThisFrame)
        {
            gp.ResetHaptics();
            Debug.Log("Rumble stopped");
        }
    }
}