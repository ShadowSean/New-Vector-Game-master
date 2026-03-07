using UnityEngine;

public class CrewTriggerArea : MonoBehaviour
{
    [SerializeField] Transform player;
    
    [SerializeField] Transform crewquatersZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.position = crewquatersZone.position;
        }
       
    }
}
