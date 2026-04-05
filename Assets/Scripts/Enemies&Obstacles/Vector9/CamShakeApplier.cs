using UnityEngine;

public class CamShakeApplier : MonoBehaviour
{
    public WalkCamShake shakeSource;
    private Vector3 baseLocalPosition;

    private void Start()
    {
        baseLocalPosition = transform.localPosition;
        RumbleManager.Instance.RumblePulse(1f, 0.8f, 0.2f);
    }

    private void LateUpdate()
    {
        if (shakeSource == null) return;
        transform.localPosition = baseLocalPosition + shakeSource.GetShakeOffset();
    }
}
