using UnityEngine;

public class MapUI : MonoBehaviour
{
    public GameObject mapUI;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mapUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        mapUI.SetActive(false);
    }
}
