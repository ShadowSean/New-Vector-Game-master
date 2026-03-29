using UnityEngine;

public class Vector9Door : MonoBehaviour
{
    public Animator doorOne;
    public Animator doorTwo;
    public Animator doorThree;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {

            doorOne.SetBool("isOpen", true);
            doorTwo.SetBool("isOpen", true);
            doorThree.SetBool("isOpen", true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        doorOne.SetBool("isOpen", false);
        doorTwo.SetBool("isOpen", false);
        doorThree.SetBool("isOpen", false);
    }
}
