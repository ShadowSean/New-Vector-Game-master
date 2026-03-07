using UnityEngine;

public class MainHallwayZone : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform enemy;
    [SerializeField] Transform mainhallwayZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.position = mainhallwayZone.position;
        }
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy entered main hallway");
            enemy.position = mainhallwayZone.position;
        }
    }
}
