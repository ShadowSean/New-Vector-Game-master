using UnityEngine;

public class PopupAnimEvent : MonoBehaviour
{
    public void uiPopUP()
    {
        RumbleManager.Instance.RumblePulse(0.1f, 0.1f, 0.2f);
    }
}
