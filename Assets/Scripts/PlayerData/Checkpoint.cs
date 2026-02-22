using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Data Settings")]
    public GameObject checkpointUI;

    [SerializeField] GameHandler gameHandler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameHandler.Save();
            StartCoroutine(CheckpointRoutine());
            
        }
    }

    IEnumerator CheckpointRoutine()
    {
        checkpointUI.SetActive(true);
        yield return new WaitForSeconds(5);
        checkpointUI.SetActive(false);
    }
}
