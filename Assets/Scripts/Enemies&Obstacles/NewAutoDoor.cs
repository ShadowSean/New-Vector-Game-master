using UnityEngine;

public class NewAutoDoor : MonoBehaviour
{
    [SerializeField] Animator animatedDoor;

    public bool newKeyFound;

    private void Start()
    {
        newKeyFound = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }
}
