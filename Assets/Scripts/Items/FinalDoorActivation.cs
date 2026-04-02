using UnityEngine;

public class FinalDoorActivation : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip alarmSound;
    private bool doorDeactivated;

    private void Update()
    {
        if (doorDeactivated) return;

        if (GeneratorCounter.Instance != null &&
            GeneratorCounter.Instance.FixedCount >= GeneratorCounter.Instance.totalGens)
        {
            animator.SetBool("isOpen", true);

            if (audioSource != null && alarmSound != null)
            {
                audioSource.clip = alarmSound;
                audioSource.loop = true;
                audioSource.Play();
            }

            doorDeactivated = true;
        }
    }
}