using UnityEngine;
using System.Collections;

public class StorageTriggerZone : MonoBehaviour
{
    [SerializeField] Transform player;
    
    [SerializeField] Transform storageZone;

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            player.position = storageZone.position;
        }
        
    }

    
}
