using UnityEngine;

public class VentAnimEvent : MonoBehaviour
{
    public void OnVentBreak()
    {
        RumbleManager.Instance.RumblePulse(1f, 0.4f, 0.2f);
    }
}
