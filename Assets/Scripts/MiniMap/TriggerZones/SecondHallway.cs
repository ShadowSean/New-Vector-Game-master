using UnityEngine;

public class SecondHallway : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform enemy;
    [SerializeField] Transform secondHallwayZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.position = secondHallwayZone.position;
        }
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy entered main hallway");
            enemy.position = secondHallwayZone.position;
        }
    }
}
