using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RaycastPickup : MonoBehaviour
{
    public float pickupRange = 3f;
    public LayerMask interactLayer;
    

    public Camera playerCamera;
    public Camera mapCam;
    [SerializeField] GameObject scope;
    public ItemSwitcher itemSwitcher;
    public GameObject map;
    public GameObject mapUI;
    public GameObject intIcon;
    [SerializeField] Light flashlightSource;

    private FPController playerMovement;
    private Animator anim;
    private LightBehaviour lightBehaviour;
    private TaserRodAttack taserValues;

    public GameObject flashlightUpgradeUI;
    public GameObject taserrodUpgradeUI;

    
    public AudioSource audioSource;
    public AudioClip pickupSound;

    private bool hasMinimap = false;
    private bool minimapOpen = false;

    public static bool canUpgradeFlash;
    public static bool canUpgradeTaser;
    

    public float minimapCD = 30f;
    private bool canOpenMap = true;




    pickupItem currentPickup;
    
    KeyCard currentKeyCard;

    private PlayerInput playerInput;

    private InputAction equipAction;
    private InputAction minimapAction;

    

    private void Awake()
    {
        playerInput = FindFirstObjectByType<PlayerInput>();
        if (playerInput != null)
        {
            equipAction = playerInput.actions["Equip Items"];
            minimapAction = playerInput.actions["MapToggle"];
        }

        else
        {
            Debug.LogWarning("RaycastPickup: Player input not found.");
        }
    }

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
            
            currentKeyCard = hit.collider.GetComponent<KeyCard>();
            

            if (currentPickup != null)
            {
               

                if (equipAction != null && equipAction.WasPressedThisFrame())
                {
                    PickUp(currentPickup);
                }
            }
          
            else if (currentKeyCard != null)
            {
                
                if (equipAction != null && equipAction.WasPressedThisFrame())
                {
                    currentKeyCard.Interact();
                    
                    currentKeyCard = null;
                }
            }
            
        }
        if (!hitSomething)
        {
            intIcon.SetActive(false);
            
            if (currentKeyCard != null)
            {
                
                currentKeyCard = null;
            }
            
            if (currentPickup != null)
            {
                currentPickup = null;
            }
        }

        
        if (hasMinimap && minimapAction != null && minimapAction.WasPressedThisFrame() && canOpenMap)
        {
            minimapOpen = !minimapOpen;
            
            map.SetActive(minimapOpen);
            mapUI.SetActive(minimapOpen);
            flashlightSource.enabled = !minimapOpen;
            scope.SetActive(!minimapOpen);

            

            if (playerMovement != null)
            {
                if (minimapOpen)
                {
                    
                    mapCam.gameObject.SetActive(true);
                    playerMovement.canMove = false;
                    anim.enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    StartCoroutine(MiniMapCooldown());
                    mapCam.gameObject.SetActive(false);
                    playerMovement.canMove = true;
                    anim.enabled = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                
            }
            

            
        }



    }

    IEnumerator MiniMapCooldown()
    {
        canOpenMap = false;
        yield return new WaitForSeconds(minimapCD);
        canOpenMap = true;
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
        StartCoroutine(FlashlightUpgradeRoutine());

        lightBehaviour.drainRate = 300;
        Debug.Log("Flashlight upgrade applied: drainRate = " + lightBehaviour.drainRate);
    

    }

    IEnumerator FlashlightUpgradeRoutine()
    {
        flashlightUpgradeUI.SetActive(true);
        yield return new WaitForSeconds(5);
        flashlightUpgradeUI.SetActive(false);
    }

    IEnumerator TaserRodUpgradeRoutine()
    {
        taserrodUpgradeUI.SetActive(true);
        yield return new WaitForSeconds(5);
        taserrodUpgradeUI.SetActive(false);
    }



    void TaserRodUpgrade()
    {
        canUpgradeTaser = true;
       if (taserValues == null)
        {
            taserValues = GetComponentInChildren<TaserRodAttack>(true);
        }

       StartCoroutine(TaserRodUpgradeRoutine());

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
                    break;
            }
        }
        
        currentPickup.gameObject.SetActive(false);
    }

    
}
