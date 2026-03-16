using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InventoryStatsHandler : MonoBehaviour
{

    [Header("Cameras")]
    [SerializeField] Camera invCam;

    [Header("Inventory UI")]
    public GameObject mainInvUI;
    public GameObject flashStats;
    public GameObject taserStats;
    public GameObject flameStats;
    public GameObject inventory;
    public GameObject stamina;
    public GameObject objectives;

    [Header("Inventory Prefabs")]
    public GameObject flashlight;
    public GameObject taserrod;
    public GameObject flamethrower;

    [Header("Pausing settings")]
    private FPController movementAndRotation;



    FPController movement;
    public bool gameisPaused;
    bool isStatsOpen;
    private PlayerInput playerInput;
    private InputAction upgradeMenuAction;
    public GameObject playerscope;

    private void Awake()
    {
        playerInput = FindFirstObjectByType<PlayerInput>();
        if (playerInput != null)
        {
            upgradeMenuAction = playerInput.actions["Upgrade Menu"];
        }
        else
        {
            Debug.LogWarning("InventoryStatsHandler: No player input was detected.");
        }
    }
    private void Start()
    {
        movement = GetComponent<FPController>();
    }
    private void Update()
    {
        if (upgradeMenuAction != null && upgradeMenuAction.WasPressedThisFrame())
        {
            isStatsOpen = !isStatsOpen;
            invCam.gameObject.SetActive(isStatsOpen);
            
            mainInvUI.gameObject.SetActive(isStatsOpen);
            playerscope.SetActive(!isStatsOpen);
            objectives.SetActive(!isStatsOpen);
            stamina.SetActive(!isStatsOpen);
            inventory.SetActive(!isStatsOpen);

            if (movement != null)
            {
                if (isStatsOpen)
                {
                    
                    Pause();
                    
                    
                }
                else
                {
                    Resume();
                    
                }
                
            }
        }
    }

    public void EnableFlashlight()
    {
        flashStats.SetActive(true);
        flashlight.SetActive(true);
        taserrod.SetActive(false);
        flamethrower.SetActive(false);
        taserStats.SetActive(false);
        flameStats.SetActive(false);
    }

    public void EnableTaser()
    {
        taserStats.SetActive(true);
        taserrod.SetActive(true);
        flashlight.SetActive(false);
        flamethrower.SetActive(false);
        flashStats.SetActive(false);
        flameStats.SetActive(false);
    }

    public void EnableFlamethrower()
    {
        flameStats.SetActive(true);
        flamethrower.SetActive(true);
        taserrod.SetActive(false);
        flashlight.SetActive(false);
        taserStats.SetActive(false);
        flashStats.SetActive(false);
    }

    public void Resume()
    {
        if (movementAndRotation != null)
        {
            movementAndRotation.canMove = true;
        }
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        Time.timeScale = 1f;
        gameisPaused = false;
    }

    public void Pause()
    {
        if (movementAndRotation != null)
        {

            movementAndRotation.canMove = false;
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerscope.SetActive(true);
        Time.timeScale = 0f;
        gameisPaused = true;
    }
}
