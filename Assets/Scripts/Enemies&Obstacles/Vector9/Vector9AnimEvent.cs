using System.Runtime.CompilerServices;
using UnityEngine;

public class Vector9AnimEvent : MonoBehaviour
{

    [Header("Proximity Step Rumble")]
    [Tooltip("Max rumble intensity when near Vector 9 walking/sprinting")]
    [Range(0f, 0.5f)] public float maxStepIntensity = 0.15f;
    [Tooltip("Should match the AudioSource maxDistance on genFixingSource")]
    public float stepMaxDistance = 20f;

    private Vector9Movement movement;
    public bool isMoving;
    public void OnVectorStep()
    {

        if (isMoving && movement != null)
        {
            float dist = Vector3.Distance(transform.position, movement.transform.position);
            float t = Mathf.Clamp01(dist / stepMaxDistance);
            float intensity = Mathf.Lerp(maxStepIntensity, 0f, t);
            RumbleManager.Instance.RumblePulse(0.8f, 0.5f, 0.2f);
        }
    }
}
