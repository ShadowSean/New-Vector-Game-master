using UnityEngine;

public class escapePodTeleport : MonoBehaviour
{
    public Transform escapePodTP;
    public Transform player;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null)
                cc.enabled = false;

            player.transform.position = escapePodTP.transform.position;

            if (cc != null)
                cc.enabled = true;
        }
    }
}
