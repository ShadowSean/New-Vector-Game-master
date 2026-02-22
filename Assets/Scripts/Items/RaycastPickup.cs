using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class RaycastPickup : MonoBehaviour
{
    public float pickupRange = 3f;
    public LayerMask interactLayer;
    public KeyCode pickupKey = KeyCode.E;

    public Camera playerCamera;
    public GameObject scope;
    public ItemSwitcher itemSwitcher;
    public GameObject minimap;
    public GameObject minimapText;
    public GameObject intIcon;

    private FPController playerMovement;
    private Animator anim;
    private LightBehaviour lightBehaviour;
    private TaserRodAttack taserValues;

    
    public AudioSource audioSource;
    public AudioClip pickupSound;

    private bool hasMinimap = false;
    private bool minimapOpen = false;
    public static bool canUpgradeFlash;
    public static bool canUpgradeTaser;
    public KeyCode minimapKey = KeyCode.M;




    pickupItem currentPickup;
    Door currentDoor;
    KeyCard currentKeyCard;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        playerMovement = GetComponent<FPController>();
        anim = GetComponentInChildren<Animator>();
    }


    private void Update()
    {
        RaycastHit hit;
        bool hitSomething = false;

        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * pickupRange, Color.green);

        if (Physics.Raycast(playerCamera.transform.position,  playerCamera.transform.forward, out hit, pickupRange, interactLayer))
        {

            hitSomething = true;
            intIcon.SetActive(true);
            currentPickup = hit.collider.GetComponent<pickupItem>();
            currentDoor = hit.collider.GetComponentInParent<Door>();
            currentKeyCard = hit.collider.GetComponent<KeyCard>();
            

            if (currentPickup != null)
            {
               

                if (Input.GetKeyDown(pickupKey))
                {
                    PickUp(currentPickup);
                }
            }
            else if (currentDoor != null)
            {
                currentDoor.ShowInteractPromt(true);
                if (Input.GetKeyDown(pickupKey))
                {
                    currentDoor.Interact();
                    
                }
            }
            else if (currentKeyCard != null)
            {
                currentKeyCard.ShowInteractPromt(true);
                if (Input.GetKeyDown(pickupKey))
                {
                    currentKeyCard.Interact();
                    currentKeyCard.ShowInteractPromt(false);
                    currentKeyCard = null;
                }
            }
            
        }
        if (!hitSomething)
        {
            intIcon.SetActive(false);
            if (currentDoor != null)
            {
                currentDoor.ShowInteractPromt(false);
                currentDoor = null;
            }
            if (currentKeyCard != null)
            {
                currentKeyCard.ShowInteractPromt(false);
                currentKeyCard = null;
            }
            
            if (currentPickup != null)
            {
                currentPickup = null;
            }
        }

        
        if (hasMinimap && Input.GetKeyDown(minimapKey))
        {
            minimapOpen = !minimapOpen;
            
            minimap.SetActive(minimapOpen);

            if (scope != null)
            {
                scope.SetActive(!minimapOpen);
            }
        }


    }

    void FlashlightUpgrade()
    {
        canUpgradeFlash = true;
        lightBehaviour = playerCamera.GetComponentInChildren<LightBehaviour>(true);

        if (lightBehaviour == null)
        {
            Debug.LogWarning("No LightBehaviour found under the player. Upgrade not applied.");
            return;
        }

        lightBehaviour.drainRate = 300;
        Debug.Log("Flashlight upgrade applied: drainRate = " + lightBehaviour.drainRate);
    

    }



    void TaserRodUpgrade()
    {
        canUpgradeTaser = true;
       if (taserValues == null)
        {
            taserValues = GetComponentInChildren<TaserRodAttack>(true);
        }

        taserValues.cooldown = 2.5f;
        taserValues.stunRange = 25f;
    }

    void PickUp(pickupItem currentPickup)
    {
       
        if (audioSource != null && pickupSound != null)
        {
            audioSource.PlayOneShot(pickupSound);
        }

        if (currentPickup.playerItems != null)
        {
            currentPickup.playerItems.SetActive(true);
        }
        if (itemSwitcher != null)
        {
            switch (currentPickup.itemType)
            {
                case ItemType.Flashlight:
                    itemSwitcher.PickupFlashlight();
                    break;
                case ItemType.Taser:
                    itemSwitcher.PickupTaser();
                    break;
                case ItemType.Flamethrower:
                    itemSwitcher.PickupFlamethrower();
                    break;
                case ItemType.TaserUpg:
                    TaserRodUpgrade();
                    break;
                case ItemType.FlashlightUpg:
                    FlashlightUpgrade();
                    break;
                case ItemType.Map:
                    hasMinimap = true;
                    StartCoroutine(MapTutorial());
                    break;
            }
        }
        
        currentPickup.gameObject.SetActive(false);
    }

    IEnumerator MapTutorial()
    {
        playerMovement.canMove = false;
        anim.enabled = false;
        minimap.SetActive(true);
        minimapText.SetActive(true);
        yield return new WaitForSeconds(5f);
        minimapText.SetActive(false);
        playerMovement.canMove = true;
        anim.enabled = true;
    }
}
