using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemSwitcher : MonoBehaviour
{
    public GameObject flashlight_player;
    public GameObject taserRod_player;
    public GameObject flamethrower_player;
    public GameObject flashlightBar;
    public Animator animator;

    public GameObject flashlightIcon;
    public GameObject taserIcon;
    public GameObject batteryInt;

    [Header("Audio Sources & Clips")]
    public AudioSource itemSounds;
    public AudioClip flashlightSound;

    [Header("Flashlight Sound Timing")]
    public float equipDelay = 0.15f;
    public float toggleDelay = 0f;
    public float soundCD = 0.08f;


    Coroutine soundRoutine;
    float nextSoundTime;
    


    private TaserRodAttack attackController;
    LightBehaviour batteryBehaviour;
    public FasterGen fastRepairSpeed;

    private int currentItemIndex = 0;
    public bool hasFlashlight = false;
    public bool hasTaser = false;
    public bool hasFlamethrower;
    bool hasBattery;
    GameObject batteryPrefab;

    private PlayerInput playerInput;
    private InputAction slot1Action;
    private InputAction slot2Action;
    private InputAction slot3Action;
    private InputAction reloadBatteryAction;
    private InputAction flashlightToggleAction;

    private void Awake()
    {
        
        playerInput = FindFirstObjectByType<PlayerInput>();

        if (playerInput != null)
        {
            Debug.Log("Found PlayerInput on: " + playerInput.gameObject.name);
            Debug.Log("Current action map: " + playerInput.currentActionMap?.name);

            slot1Action = playerInput.actions["Weapon Slot 1"];
            slot2Action = playerInput.actions["Weapon Slot 2"];
            slot3Action = playerInput.actions["Weapon Slot 3"];
            reloadBatteryAction = playerInput.actions["Recharge Battery"];
            flashlightToggleAction = playerInput.currentActionMap.FindAction("Flashlight Toggle");

            Debug.Log("slot1Action null? " + (slot1Action == null));
            Debug.Log("slot2Action null? " + (slot2Action == null));
            Debug.Log("slot3Action null? " + (slot3Action == null));
            Debug.Log("reloadBatteryAction null? " + (reloadBatteryAction == null));
            Debug.Log("flashlightToggleAction null? " + (flashlightToggleAction == null));
        }

        else
        {
            Debug.LogWarning("Itemswitcher: Player input has not been found.");
        }
    }
    private void Start()
    {
        batteryBehaviour = flashlight_player.GetComponentInChildren<LightBehaviour>();
        attackController = taserRod_player.GetComponent<TaserRodAttack>();
        
        if (itemSounds == null)
        {
            itemSounds = GetComponent<AudioSource>();
        }
       
    }
    private void Update()
    {
        if (slot1Action != null && slot1Action.WasPressedThisFrame() && hasFlashlight)
        {
            flashlight_player.SetActive(true);
            animator.SetTrigger("Take Out");
           
            EquipItem(1);
        }

        if (slot2Action != null && slot2Action.WasPressedThisFrame() && hasTaser)
        {

            animator.SetTrigger("Hide");
            

            EquipItem(2);
        }

        if (slot3Action != null && slot3Action.WasPressedThisFrame() && hasFlamethrower)
        {
            EquipItem(3);
        }

        if (reloadBatteryAction != null && reloadBatteryAction.WasPressedThisFrame() && hasFlashlight && hasBattery)
        {
            if (batteryBehaviour != null)
            {
                batteryBehaviour.ReplaceBatteryFull();
            }

            //Counts this as collecting battery
            if (fastRepairSpeed != null)
            {
                fastRepairSpeed.batteryCount++;
                Debug.Log("BatteryCount is now: " + fastRepairSpeed.batteryCount);
            }
            else
            {
                Debug.LogWarning("ItemSwitched: fastRepaidSpeed NOT assigned");
            }
            
            animator.SetTrigger("Recharage");
            EquipItem(1);

            if (batteryPrefab != null)
            {
                batteryPrefab.SetActive(false);
                batteryInt.SetActive(false);
            }
            hasBattery = false;
            batteryPrefab = null;
        }

        if (flashlightToggleAction != null && flashlightToggleAction.WasPressedThisFrame())
        {
            //Only happens when flashlight is equipped
            if (currentItemIndex != 1) return;

            if (batteryBehaviour != null)
            {
                batteryBehaviour.ToggleFlashlight();

                bool isOn = batteryBehaviour.IsFlashlightOn();
                animator.SetBool("On", isOn);
            }
            else
            {
                Debug.LogWarning("Item Switcher: Battery Behaviour does not exist.");
            }

           

            PlayFlashlightSFX(toggleDelay);
        }
        

    }

    public void ToggleFlashlight()
    {

    }

    void PlayFlashlightSFX(float delay)
    {
        if (itemSounds == null || flashlightSound == null) return;
        if (Time.time < nextSoundTime) return;

        nextSoundTime = Time.time + soundCD;

        if (soundRoutine != null) StopCoroutine(soundRoutine);
        soundRoutine = StartCoroutine(PlayFlashlightSFXRoutine(delay));
    }

    IEnumerator PlayFlashlightSFXRoutine(float delay)
    {
        if(delay > 0) yield return new WaitForSeconds(delay);
        itemSounds.PlayOneShot(flashlightSound);
    }

    void EquipItem(int index)
    {
        currentItemIndex = index;

        flashlight_player.SetActive(index == 1);
        taserRod_player.SetActive(index == 2);
        flamethrower_player.SetActive(index == 3);

        if (flashlightBar != null)
        {
            flashlightBar.SetActive(index == 1);
        }


        UpdateIcons();
    }

    void CycleItems(int direction)
    {
        int maxItems = (hasFlashlight ? 1 : 0) + (hasTaser ? 1 : 0) + (hasFlamethrower ? 1: 0);
        if (maxItems <= 1) return;

        int[] availableItems = new int[maxItems];
        int count = 0;
        if (hasFlashlight) availableItems[count++] = 1;
        if(hasTaser) availableItems[count++] = 2;
        if (hasFlamethrower) availableItems[count++] = 3;
        
        int currentIndex = System.Array.IndexOf(availableItems, currentItemIndex);
        currentIndex = (currentIndex + direction + count) % count;
        EquipItem(availableItems[currentIndex]);
    }

    public void PickupFlashlight()
    {
        hasFlashlight = true;
        UpdateIcons();
        EquipItem(1);
    }

    public void PickupTaser()
    {
        hasTaser = true;
        UpdateIcons();
        EquipItem(2);
    }

    public void PickupFlamethrower()
    {
        hasFlamethrower = true;
        EquipItem(3);
    }

    void UpdateIcons()
    {
        if (flashlightIcon != null)
        {
            flashlightIcon.SetActive(hasFlashlight);
        }

        if (taserIcon != null)
        {
            taserIcon.SetActive(hasTaser);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
            hasBattery = true;
            batteryPrefab = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
            hasBattery = false;
            if (batteryPrefab == other.gameObject)
            {
                batteryPrefab = null;
            }
        }
    }
}
