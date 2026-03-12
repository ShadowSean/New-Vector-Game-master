using UnityEngine;
using System.Collections;

public class StorageTriggerZone : MonoBehaviour
{
    [SerializeField] Transform player;
    
    [SerializeField] Transform storageZone;

    [SerializeField] GameObject aiTitle;

    [SerializeField] GameObject storageText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(StorageAIText());
            player.position = storageZone.position;
        }
        
    }

    IEnumerator StorageAIText()
    {
        aiTitle.SetActive(true);
        yield return new WaitForSeconds(2);
        storageText.SetActive(true);
        yield return new WaitForSeconds(10);
        aiTitle.SetActive(false);
        storageText.SetActive(false);
    }
}
