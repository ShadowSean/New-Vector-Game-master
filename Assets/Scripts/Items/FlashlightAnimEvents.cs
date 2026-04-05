using UnityEngine;

public class FlashlightAnimEvents : MonoBehaviour
{
    public void TriggerReloadRumble()
    {
        RumbleManager.Instance.RumblePulse(0.4f, 0.3f, 0.2f);
    }
}