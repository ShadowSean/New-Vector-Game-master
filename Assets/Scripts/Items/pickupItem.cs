using System.ComponentModel;
using UnityEngine;

public class pickupItem : MonoBehaviour
{
    public GameObject playerItems;
    public string itemName;

    public ItemType itemType;

    [HideInInspector] public bool wasCollected;

    bool playerInRange;


    public void SetCollectedState(bool collected)
    {
        wasCollected = collected;
        gameObject.SetActive(!collected);
    }

    public void PlayPickupRumble()
    {
        switch (itemType)
        {
            case ItemType.Flashlight:
            case ItemType.Map:
                RumbleManager.Instance.RumblePulse(0.1f, 0.5f, 0.12f);
                break;
            case ItemType.Taser:
            case ItemType.Flamethrower:
                RumbleManager.Instance.RumblePulse(0.5f, 0.9f, 0.2f);
                break;
            case ItemType.FlashlightUpg:
            case ItemType.TaserUpg:
                RumbleManager.Instance.RumbleFadeOut(0.3f, 0.8f, 0.3f);
                break;
            default:
                RumbleManager.Instance.RumblePulse(0.2f, 0.6f, 0.15f);
                break;
        }
    }

    public bool GetCollectedState()
    {
        return wasCollected;
    }
    

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
