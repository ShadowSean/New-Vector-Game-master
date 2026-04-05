using UnityEngine;

public class FinalDoorPlayer : MonoBehaviour
{
    [SerializeField] Animator doorAnim;
    [SerializeField] AudioSource doorSource;
    [SerializeField] AudioClip doorOpenClip;
    [SerializeField] AudioClip doorCloseClip;

    

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorSource.PlayOneShot(doorOpenClip);
            doorAnim.SetBool("Open", true);
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
