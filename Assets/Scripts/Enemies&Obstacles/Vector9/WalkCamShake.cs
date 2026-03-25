using System.Collections;
using UnityEngine;

public class WalkCamShake : MonoBehaviour
{
    [Header("Walk Camera Screen Shake")]
    public bool startShake;
    public AnimationCurve curve;
    public float duration = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (startShake)
        {
            startShake = false;
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedtime = 0f;

        while (elapsedtime < duration)
        {
            elapsedtime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedtime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPosition;
    }
}
