using UnityEngine;

public class firstFloorTP : MonoBehaviour
{
    public Transform firstFloor;
    public Transform player;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null)
                cc.enabled = false;

            player.transform.position = firstFloor.transform.position;

            if (cc != null)
                cc.enabled = true;
        }
    }
}
