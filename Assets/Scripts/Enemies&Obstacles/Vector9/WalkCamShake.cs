using System.Collections;
using UnityEngine;

public class WalkCamShake : MonoBehaviour
{
    [Header("Walk Camera Screen Shake")]
    public bool startShake;
    public AnimationCurve curve;
    public float duration = 1.0f;
    private Vector3 shakeOffset = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (startShake)
        {
            startShake = false;
            StopAllCoroutines();
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking()
    {
        
        float elapsedtime = 0f;

        while (elapsedtime < duration)
        {
            elapsedtime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedtime / duration);
            shakeOffset = Random.insideUnitSphere * strength;
            yield return null;
        }
        shakeOffset = Vector3.zero;
    }

    public Vector3 GetShakeOffset() => shakeOffset;
}
