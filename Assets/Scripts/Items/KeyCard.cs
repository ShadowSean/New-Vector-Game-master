using UnityEngine;

public class KeyCard : MonoBehaviour
{

    [SerializeField] Animator animator;
    [SerializeField] AudioSource doorSource;
    [SerializeField] AudioClip doorOpen;
    
    public GameObject keycardObject;

    public void Interact()
    {
        doorSource.PlayOneShot(doorOpen);
        animator.SetBool("isOpen", true);
        keycardObject.SetActive(false);
    }

    
}
