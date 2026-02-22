using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{


    public Transform secondFloor;
    public Transform player;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null)
                cc.enabled = false; 

            player.transform.position = secondFloor.transform.position;

            if (cc != null)
                cc.enabled = true; 
        }
    }

}
