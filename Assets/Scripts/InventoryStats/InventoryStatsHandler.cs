using UnityEngine;

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

    FPController movement;
    bool isStatsOpen;
    public KeyCode inventoryStats = KeyCode.Tab;
    public GameObject playerscope;

    private void Start()
    {
        movement = GetComponent<FPController>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(inventoryStats))
        {
            isStatsOpen = !isStatsOpen;
            invCam.gameObject.SetActive(isStatsOpen);
            playerscope.gameObject.SetActive(isStatsOpen);
            mainInvUI.gameObject.SetActive(isStatsOpen);
            objectives.SetActive(!isStatsOpen);
            stamina.SetActive(!isStatsOpen);
            inventory.SetActive(!isStatsOpen);

            if (movement != null)
            {
                if (isStatsOpen)
                {
                    movement.canMove = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    
                }
                else
                {
                    movement.canMove = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
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
}
