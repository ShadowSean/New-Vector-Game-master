using UnityEngine;
using System.Collections;

public class ControlRoomZone : MonoBehaviour
{
    [SerializeField] Transform player;
    
    [SerializeField] Transform controlRoomZone;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            player.position = controlRoomZone.position;
        }
        
    }

    
}
