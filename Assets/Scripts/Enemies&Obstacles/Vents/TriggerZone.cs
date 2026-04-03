using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerZone : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Animator on the target GameObject to animate.")]
    public Animator animator;

    [Tooltip("AudioSource to play the sound from.")]
    public AudioSource audioSource;

    [Header("Settings")]
    [Tooltip("Exact name of the Animator trigger parameter to fire.")]
    public string animationTriggerName = "Play";

    [Tooltip("Tag that the entering collider must have to activate this trigger.")]
    public string playerTag = "Player";

    private bool hasTriggered = false;

    private void Reset()
    {
        BoxCollider bc = GetComponent<BoxCollider>();
        if (bc != null) bc.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag(playerTag)) return;

        hasTriggered = true;

        PlayAnimation();
        PlaySound();
    }

    private void PlayAnimation()
    {
        if (animator == null)
        {
            Debug.LogWarning($"[TriggerZone] No Animator assigned on '{gameObject.name}'.");
            return;
        }

        Debug.Log($"Firing '{animationTriggerName}' on {animator.gameObject.name}");
        animator.SetTrigger(animationTriggerName);
    }

    private void PlaySound()
    {
        if (audioSource == null)
        {
            Debug.LogWarning($"[TriggerZone] No AudioSource assigned on '{gameObject.name}'.");
            return;
        }

        if (audioSource.clip == null)
        {
            Debug.LogWarning($"[TriggerZone] AudioSource on '{gameObject.name}' has no clip assigned.");
            return;
        }

        audioSource.Play();
    }

    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}