using UnityEngine;
using TMPro;

public class PopupTrigger : MonoBehaviour
{
    [Header("Popup Settings")]
    [SerializeField] string message = "It's Locked";
    [SerializeField] GameObject popupPrompt;
    [SerializeField] TMP_Text popupText;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GeneratorCounter.Instance != null &&
            GeneratorCounter.Instance.FixedCount >= GeneratorCounter.Instance.totalGens) return;

        if (popupText != null) popupText.text = message;
        if (popupPrompt != null) popupPrompt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (popupPrompt != null) popupPrompt.SetActive(false);
    }
}