using UnityEngine;

public class VentAnimatorBridge : MonoBehaviour
{
    public VentLogic ventLogic;

    public void OnVentAnimationComplete()
    {
        if (ventLogic != null)
        {
            ventLogic.UnlockVent();
        }
    }
}