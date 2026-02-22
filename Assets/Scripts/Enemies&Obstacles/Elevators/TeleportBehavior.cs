using UnityEngine;

public class TeleportBehavior : MonoBehaviour
{
    public Transform player;
    public Transform firstFloor;
    public Transform secondFloor;


    public void TeleportOne()
    {
        player.transform.position = firstFloor.transform.position;

    }

    public void TeleportTwo()
    {
        player.transform.position = secondFloor.transform.position;
    }
}
