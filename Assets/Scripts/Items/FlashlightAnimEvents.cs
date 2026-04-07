using UnityEngine;

public class FlashlightAnimEvents : MonoBehaviour
{
    [SerializeField] ItemSwitcher swap;
    public void TriggerReloadRumble()
    {
        RumbleManager.Instance.RumblePulse(0.4f, 0.3f, 0.2f);
    }

    public void Swao()
    {
        swap.Swap(true);
    }

    public void SwaoFalse()
    {
        swap.Swap(false);
       
    }



}