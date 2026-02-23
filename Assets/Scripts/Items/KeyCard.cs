using UnityEngine;

public class KeyCard : MonoBehaviour
{

    [SerializeField] Animator animator;
    public GameObject keycardObject;

    public void Interact()
    {
        animator.SetBool("isOpen", true);
        keycardObject.SetActive(false);
    }

    
}
