using System.Collections;
using UnityEngine;

public class CrewTriggerArea : MonoBehaviour
{
    [SerializeField] Transform player;
    
    [SerializeField] Transform crewquatersZone;

    [SerializeField] GameObject crewText;
    [SerializeField] GameObject aiTitle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CrewAIText());
            player.position = crewquatersZone.position;
        }
       
    }

    IEnumerator CrewAIText()
    {
        aiTitle.SetActive(true);
        yield return new WaitForSeconds(2);
        crewText.SetActive(true);
        yield return new WaitForSeconds(10);
        aiTitle.SetActive(false);
        crewText.SetActive(false);
    }
}
