using UnityEngine;
using System.Collections;

public class EscapeBayZone : MonoBehaviour
{
    public GameObject escapebaytext;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(EscapeText());
        }
    }

    IEnumerator EscapeText()
    {
        escapebaytext.SetActive(true);
        yield return new WaitForSeconds(5f);
        escapebaytext.SetActive(false);
    }
}
