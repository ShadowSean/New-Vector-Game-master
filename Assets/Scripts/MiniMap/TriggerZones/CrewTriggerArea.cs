using System.Collections;
using UnityEngine;

public class CrewTriggerArea : MonoBehaviour
{
    public Animator doorAnim;
    public GameObject crewquatertext;

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {

            doorAnim.SetBool("isOpen", true);
        }

        if (other.CompareTag("Player"))
        {
            StartCoroutine(CrewText());
        }
       
    }

    IEnumerator CrewText()
    {
        crewquatertext.SetActive(true);
        yield return new WaitForSeconds(5f);
        crewquatertext.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        doorAnim.SetBool("isOpen", false);
    }


}
