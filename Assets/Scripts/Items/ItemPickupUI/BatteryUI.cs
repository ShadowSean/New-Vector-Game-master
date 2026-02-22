using UnityEngine;

public class BatteryUI : MonoBehaviour
{
    public GameObject batteryUI;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            batteryUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        batteryUI.SetActive(false);
    }
}
