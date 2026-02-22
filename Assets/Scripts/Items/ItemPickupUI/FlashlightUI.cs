using UnityEngine;

public class FlashlightUI : MonoBehaviour
{
    public GameObject flashlightUI;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            flashlightUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        flashlightUI.SetActive(false);
    }
}
