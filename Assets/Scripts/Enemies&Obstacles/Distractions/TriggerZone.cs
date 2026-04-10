using System.Collections;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class TriggerZone : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Animator on the target GameObject to animate.")]
    public Animator animator;
    public GameObject ventlight;
    [Tooltip("AudioSource to play the sound from.")]
    public AudioSource audioSource;

    private bool hasTriggered = false;

    private void Start()
    {
        Debug.Log(gameObject);
    }

    private void Reset()
    {
        BoxCollider bc = GetComponent<BoxCollider>();
        if (bc != null) bc.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Barel trigger entered");

        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            ventlight.SetActive(true);
            PlayAnimation();
            PlaySound();
            Debug.Log("Valid");
        }
    }

    public void PlayAnimation()
    {
        if (animator == null)
        {
            Debug.LogWarning($"[TriggerZone] No Animator assigned on '{gameObject.name}'.");
            return;
        }
        animator.SetBool("Roll", true);
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
}