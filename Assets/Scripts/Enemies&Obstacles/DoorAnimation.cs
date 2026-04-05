using UnityEngine;
using TMPro;
using System.Collections;

public class DoorAnimation : MonoBehaviour
{
    [SerializeField] Animator doorAnim;
    [SerializeField] AudioSource doorSource;
    [SerializeField] AudioClip doorOpenClip;
    [SerializeField] AudioClip doorCloseClip;

    [Header("Generator Lock")]
    [SerializeField] GeneratorLogic requiredGenerator;
    [SerializeField] GameObject lockedPrompt;

    [Header("Door Rumble")]
    public float slideDuration = 2f;

    private bool IsUnlocked => requiredGenerator == null || requiredGenerator.GetFixedState();

    private Coroutine doorRumbleRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!IsUnlocked)
            {
                if (lockedPrompt != null) lockedPrompt.SetActive(true);
                RumbleManager.Instance.RumblePulse(0.3f, 0.5f, 0.1f);
                return;
            }

            doorSource.PlayOneShot(doorOpenClip);
            doorAnim.SetBool("isOpen", true);
            StartDoorRumble();
        }

        if (other.CompareTag("Enemy"))
        {
            doorSource.PlayOneShot(doorOpenClip);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (lockedPrompt != null) lockedPrompt.SetActive(false);
            if (!IsUnlocked) return;

            doorSource.PlayOneShot(doorCloseClip);
            doorAnim.SetBool("isOpen", false);
            StartDoorRumble();
        }

        if (other.CompareTag("Enemy"))
        {
            doorSource.PlayOneShot(doorCloseClip);
        }
    }

    void StartDoorRumble()
    {
        if (doorRumbleRoutine != null)
            StopCoroutine(doorRumbleRoutine);

        doorRumbleRoutine = StartCoroutine(DoorRumbleRoutine());
    }

    IEnumerator DoorRumbleRoutine()
    {
        RumbleManager.Instance.RumbleConstant(0.2f, 0.6f);
        yield return new WaitForSecondsRealtime(slideDuration);

        RumbleManager.Instance.RumblePulse(0.8f, 0.3f, 0.12f);
    }
}