using UnityEngine;
using TMPro;

public class DoorAnimation : MonoBehaviour
{
    [SerializeField] Animator doorAnim;
    [SerializeField] AudioSource doorSource;
    [SerializeField] AudioClip doorOpenClip;
    [SerializeField] AudioClip doorCloseClip;

    [Header("Generator Lock")]
    [SerializeField] GeneratorLogic requiredGenerator;
    [SerializeField] GameObject lockedPrompt;

    private bool IsUnlocked => requiredGenerator == null || requiredGenerator.GetFixedState();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (!IsUnlocked)
            {
                if (lockedPrompt != null) lockedPrompt.SetActive(true);
                return;
            }

            doorSource.PlayOneShot(doorOpenClip);
            doorAnim.SetBool("isOpen", true);
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
            doorAnim.SetBool("isOpen",false);
        }
        if (other.CompareTag("Enemy"))
        {
            doorSource.PlayOneShot(doorCloseClip);
        }
    }
}
