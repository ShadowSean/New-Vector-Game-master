using UnityEngine;

public class GenLightTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    public GameObject genlight;
    public CrateUI canTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (canTrigger.partsCollected)
            {
                genlight.SetActive(true);
            }
        }
    }
}
