using UnityEngine;
using System.Collections;

public class StorageTriggerZone : MonoBehaviour
{
    public GameObject storagetext;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(StorageText());
        }
        
    }

    IEnumerator StorageText()
    {
        storagetext.SetActive(true);
        yield return new WaitForSeconds(5f);
        storagetext.SetActive(false);
    }


}
