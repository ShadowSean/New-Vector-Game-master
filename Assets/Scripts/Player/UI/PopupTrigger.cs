using UnityEngine;
using TMPro;

public class PopupTrigger : MonoBehaviour
{
    [Header("Popup Settings")]
    [SerializeField] string message = "It's Locked";
    [SerializeField] GameObject popupPrompt;
    [SerializeField] TMP_Text popupText;

    [Header("Sound Settings")]
    [SerializeField] AudioClip triggerSound;
    [SerializeField] AudioSource audioSource;
    [Range(0f, 1f)]
    [SerializeField] float volume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (GeneratorCounter.Instance != null &&
            GeneratorCounter.Instance.FixedCount >= GeneratorCounter.Instance.totalGens) return;

        if (popupText != null) popupText.text = message;
        if (popupPrompt != null) popupPrompt.SetActive(true);

        PlayTriggerSound();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (popupPrompt != null) popupPrompt.SetActive(false);
    }

    private void PlayTriggerSound()
    {
        if (triggerSound == null) return;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(triggerSound, volume);
        }
        else
        {
            AudioSource.PlayClipAtPoint(triggerSound, transform.position, volume);
        }
    }
}