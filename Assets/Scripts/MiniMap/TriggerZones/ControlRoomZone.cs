using UnityEngine;
using System.Collections;

public class ControlRoomZone : MonoBehaviour
{
    public Animator doorAnim;

    public GameObject controltext;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {

            doorAnim.SetBool("isOpen", true);
        }

        if (other.CompareTag("Player"))
        {
            StartCoroutine(ControlText());
        }

    }

    IEnumerator ControlText()
    {
        controltext.SetActive(true);
        yield return new WaitForSeconds(5f);
        controltext.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        doorAnim.SetBool("isOpen", false);
    }

}
