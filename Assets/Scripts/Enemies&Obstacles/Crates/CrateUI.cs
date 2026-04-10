using UnityEngine;
using UnityEngine.InputSystem;


public class CrateUI : MonoBehaviour
{
    public GameObject crateui, equipIcon,playerCursor;

   

    public bool partsCollected;
    
    public GameObject itemRotation;

    [SerializeField] private GameObject crateObject;
    public GameObject inventory;
    public GameObject stamAndBattery;
    public GameObject objectives;

    [SerializeField] Light crateLight;

    public bool crateDisabledAfterClaim;

    bool inRange;
    bool itemEquipped;

    private FPController cameraMovement;

    private PlayerInput playerInput;
    private InputAction clickAction;
    InputActionMap iam;


    private void Awake()
    {
        playerInput = FindFirstObjectByType<PlayerInput>();
        iam = playerInput.currentActionMap;

        if (playerInput != null)
        {
            clickAction = playerInput.actions["Weapon Use"];
        }
    }

    private void Start()
    {
        if (crateObject == null)
        {
            crateObject = gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (crateDisabledAfterClaim)
        {
            return;
        }

        cameraMovement = other.GetComponent<FPController>();

        if (cameraMovement != null)
        {
            cameraMovement.DisableLook();
        }
        inRange = true;
        crateLight.enabled = true;
        playerCursor.SetActive(false);
        objectives.SetActive(false);
        stamAndBattery.SetActive(false);
        inventory.SetActive(false);
        itemRotation.SetActive(true);
        crateui.SetActive(true);
        equipIcon.SetActive(true);

        iam.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }


    private void OnTriggerStay(Collider other)
    {
        if (!inRange || !other.CompareTag("Player"))
        {
            return;
        }

    }


    public void ExitCrateUI()
    {
        itemEquipped = true;
        partsCollected = true;
        SparePartsCounter.Instance.AddPart();
        if (cameraMovement != null)
        {
            cameraMovement.RestoreLook();
        }

        inRange = false;
        crateLight.enabled = false;
        crateui.SetActive(false );
        equipIcon.SetActive(false);
        itemRotation.SetActive(false);
        objectives.SetActive(true);
        stamAndBattery.SetActive(true);
        inventory.SetActive(true);
        RumbleManager.Instance.RumblePulse(0.5f, 0.9f, 0.2f);
        playerCursor.SetActive(true );

        iam.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (itemEquipped)
        {
            crateDisabledAfterClaim = true;

            if (crateObject != null)
            {
                crateObject.SetActive(false);
            }
        }
    }

    public void SetCrateDisabledState(bool disabled)
    {
        crateDisabledAfterClaim = disabled;
        if (crateObject != null)
        {
            crateObject.SetActive(!disabled);
        }
    }

    public bool GetCrateDisabledState()
    {
        return crateDisabledAfterClaim;
    }

    
}
