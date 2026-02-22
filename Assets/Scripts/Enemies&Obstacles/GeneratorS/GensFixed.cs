using UnityEngine;

public class GensFixed : MonoBehaviour
{
    public GameObject door;
    private bool opened;

    private void Update()
    {
        if (opened) return;

        if (GeneratorCounter.Instance != null &&
            GeneratorCounter.Instance.FixedCount >= GeneratorCounter.Instance.totalGens)
        {
            door.SetActive(false);
            opened = true;
        }
    }
}
