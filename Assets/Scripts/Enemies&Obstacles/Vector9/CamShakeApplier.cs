using UnityEngine;

public class CamShakeApplier : MonoBehaviour
{
    public WalkCamShake shakeSource;
    private Vector3 baseLocalPosition;

    private void Start()
    {
        baseLocalPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        if (shakeSource == null) return;
        transform.localPosition = baseLocalPosition + shakeSource.GetShakeOffset();
    }
}
