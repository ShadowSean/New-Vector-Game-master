using UnityEngine;
using System.Collections;

public class MaintenanceBay : MonoBehaviour
{
    public GameObject maintenancetext;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CrewText());
        }
    }

    IEnumerator CrewText()
    {
        maintenancetext.SetActive(true);
        yield return new WaitForSeconds(5f);
        maintenancetext.SetActive(false);
    }
}
