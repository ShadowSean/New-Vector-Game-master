using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private EscapeCodes escapeCodes;

    private void Start()
    {
        escapeCodes = FindFirstObjectByType<EscapeCodes>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (escapeCodes != null)
        {
            escapeCodes.OnPlayerEnteredTrigger(other);
        }
    }
}
