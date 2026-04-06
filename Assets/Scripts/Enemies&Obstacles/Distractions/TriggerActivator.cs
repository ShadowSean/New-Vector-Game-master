using UnityEngine;
using System.Collections;

public class TriggerActivator : MonoBehaviour
{
    [Header("Target")]
    public GameObject objectToActivate;
    public AudioSource audioSource;
    public AudioSource audioStart;

    [Header("Options")]
    public string playerTag = "Player";
    public bool triggerOnlyOnce = true;

    [Header("Proximity Rumble")]
    public float maxRumbleDistance = 10f;
    public float rumbleLow = 0.8f;
    public float rumbleHigh = 0.4f;

    private bool hasTriggered = false;
    private Transform playerTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (triggerOnlyOnce && hasTriggered) return;

        hasTriggered = true;
        playerTransform = other.transform;

        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        if (audioStart != null)
        {
            audioStart.dopplerLevel = 0f;
            audioStart.spatialBlend = 1f;
            audioStart.rolloffMode = AudioRolloffMode.Linear;
            audioStart.minDistance = 1f;
            audioStart.maxDistance = maxRumbleDistance;
            audioStart.PlayOneShot(audioStart.clip);
            
            RumbleManager.Instance.RumblePulse(1f, 0.8f, 0.2f);
        }

        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.dopplerLevel = 0f;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.minDistance = 1f;
            audioSource.maxDistance = maxRumbleDistance;
            audioSource.Play();
            StartCoroutine(ProximityRumble());
        }
    }

    private IEnumerator ProximityRumble()
    {
        while (audioSource.isPlaying)
        {
            if (playerTransform != null)
            {
                float distance = Vector3.Distance(playerTransform.position, audioSource.transform.position);
                float proximity = 1f - Mathf.Clamp01(distance / maxRumbleDistance);

                if (proximity > 0f)
                    RumbleManager.Instance.RumblePulse(rumbleLow * proximity, rumbleHigh * proximity, 0.1f);
            }

            yield return new WaitForSeconds(0.1f);
        }

        RumbleManager.Instance.StopRumble();
    }
}