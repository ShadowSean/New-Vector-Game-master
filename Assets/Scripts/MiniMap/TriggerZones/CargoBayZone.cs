using UnityEngine;

public class CargoBayZone : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform enemy;
    [SerializeField] Transform cargoBayZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.position = cargoBayZone.position;
        }
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy has entered Cargo Bay");
            enemy.position = cargoBayZone.position;
        }
    }
}
