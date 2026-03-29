using UnityEngine;
using System.Collections;

public class ControlRoomZone : MonoBehaviour
{
    public Animator doorAnim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {

            doorAnim.SetBool("isOpen", true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        doorAnim.SetBool("isOpen", false);
    }

}
