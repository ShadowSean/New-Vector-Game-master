using UnityEngine;

public class BarrelAnimEvents : MonoBehaviour
{
    public void OnBarrelLand()
    {
        RumbleManager.Instance.RumblePulse(0.8f, 0.4f, 0.2f);
    }

    public void OnBarrelRoll()
    {
        RumbleManager.Instance.RumbleConstant(0.15f, 0.25f);
    }
}
