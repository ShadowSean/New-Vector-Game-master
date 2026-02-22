using UnityEngine;

public class itemTriggerZone : MonoBehaviour
{
    public Light tutorialLightone, tutorialLighttwo, tutorialLightthree;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialLightone.enabled = false;
            tutorialLighttwo.enabled = false;
            tutorialLightthree.enabled = false;
        }
       
    }
}
