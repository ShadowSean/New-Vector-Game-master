using UnityEngine;
using System.Collections;

public class ControlRoomZone : MonoBehaviour
{
    [SerializeField] Transform player;
    
    [SerializeField] Transform controlRoomZone;

    [SerializeField] GameObject controlText;

    [SerializeField] GameObject aiTitle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ControlAIText());
            player.position = controlRoomZone.position;
        }
        
    }

    IEnumerator ControlAIText()
    {
        aiTitle.SetActive(true);
        yield return new WaitForSeconds(2);
        controlText.SetActive(true);
        yield return new WaitForSeconds(10);
        aiTitle.SetActive(false);
        controlText.SetActive(false);
    }
}
