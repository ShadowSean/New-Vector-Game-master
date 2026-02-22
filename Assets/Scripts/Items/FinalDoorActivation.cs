using UnityEngine;

public class FinalDoorActivation : MonoBehaviour
{
    public GameObject finalDoor;
    private bool doorDeactivated;

    private void Update()
    {
        if (doorDeactivated) return;

        if (GeneratorCounter.Instance != null &&
            GeneratorCounter.Instance.FixedCount >= GeneratorCounter.Instance.totalGens)
        {
            finalDoor.SetActive(false);
            doorDeactivated = true;
        }
    }
}
