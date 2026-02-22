using System.ComponentModel;
using UnityEngine;

public class pickupItem : MonoBehaviour
{
    public GameObject playerItems;
    public string itemName;

    public ItemType itemType;

   

    bool playerInRange;

    

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            
        }
    }

    public bool isPlayerInRange()
    {
        return playerInRange;
    }
}

public enum ItemType
{
    None,
    Flashlight,
    Taser,
    Map,
    FlashlightUpg,
    TaserUpg,
    Flamethrower
   
}
