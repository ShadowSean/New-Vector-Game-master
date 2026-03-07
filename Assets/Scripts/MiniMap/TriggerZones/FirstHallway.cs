using UnityEngine;

public class FirstHallway : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform enemy;
    [SerializeField] Transform firsthallwayZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.position = firsthallwayZone.position;
        }
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy entered main hallway");
            enemy.position = firsthallwayZone.position;
        }
    }
}
