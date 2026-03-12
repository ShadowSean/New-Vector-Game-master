using UnityEngine;
using System.Collections;
public class CargoBayZone : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform enemy;
    [SerializeField] Transform cargoBayZone;

    [SerializeField] GameObject aiTitle;
    [SerializeField] GameObject cargoText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CargoAIText());
            player.position = cargoBayZone.position;
        }
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy has entered Cargo Bay");
            enemy.position = cargoBayZone.position;
        }
    }

    IEnumerator CargoAIText()
    {
        aiTitle.SetActive(true);
        yield return new WaitForSeconds(2);
        cargoText.SetActive(true);
        yield return new WaitForSeconds(10);
        aiTitle.SetActive(false);
        cargoText.SetActive(false);
    }
}
