using UnityEngine;

public class FinalDoorPlayer : MonoBehaviour
{
    [SerializeField] Animator doorAnim;
    [SerializeField] AudioSource doorSource;
    [SerializeField] AudioClip doorOpenClip;
    [SerializeField] AudioClip doorCloseClip;
    [Header("Door Rumble")]
    public float slideDuration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorSource.PlayOneShot(doorOpenClip);
            doorAnim.SetBool("Open", true);
            RumbleManager.Instance.RumbleSequence(0.2f, 0.6f, slideDuration, 1f, 0.8f, 0.2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorSource.PlayOneShot(doorCloseClip);
            doorAnim.SetBool("Open", false);
        }
    }
}