using UnityEngine;

public class TaserrodLogic : MonoBehaviour
{
    public GameObject taserRod_ground, inticon, taserRod_player, playerCursor, taserIcon;

    private ItemSwitcher itemSwitcher;

    private void Start()
    {
        itemSwitcher = FindFirstObjectByType<ItemSwitcher>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            inticon.SetActive(true);
            playerCursor.SetActive(false);
            if (Input.GetKeyDown(KeyCode.E))
            {
                taserRod_ground.SetActive(false);
                inticon.SetActive(false);
                playerCursor.SetActive(true);
                taserRod_player.SetActive(true);
                taserIcon.SetActive(true);

                itemSwitcher?.PickupTaser();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inticon.SetActive(false);
            playerCursor.SetActive(true);
        }
    }
}
