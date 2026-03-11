using UnityEngine;
using UnityEngine.InputSystem;

public class CrateFourUI : MonoBehaviour
{
    public GameObject crateui, equipIcon, playerCursor;

    public static bool partsCollectedFour;

    bool inRange;
    bool itemEquipped;

    public GameObject itemRotation;

    private FPController cameraMovement;

    private PlayerInput playerInput;
    private InputAction clickAction;

    private void Awake()
    {
        playerInput = FindFirstObjectByType<PlayerInput>();

        if (playerInput != null)
        {
            clickAction = playerInput.actions["Weapon Use"];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraMovement = other.GetComponent<FPController>();
            if (cameraMovement != null)
            {
                cameraMovement.DisableLook();
            }
            inRange = true;
            playerCursor.SetActive(false);
            crateui.SetActive(true);
            equipIcon.SetActive(true);
            itemRotation.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (inRange && other.CompareTag("Player"))
        {
            if (clickAction != null && clickAction.WasPressedThisFrame() && !itemEquipped)
            {
                if (cameraMovement != null)
                {
                   cameraMovement.RestoreLook();
                }
                playerCursor.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                equipIcon.SetActive(false);
                itemRotation.SetActive(false);
                itemEquipped = true;
                partsCollectedFour = true;
                SparePartsCounter.Instance.AddPart();
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (cameraMovement != null)
            {
                cameraMovement.RestoreLook();
            }
            inRange = false;
            crateui.SetActive(false);
            equipIcon.SetActive(false);
            itemRotation.SetActive(false);
            playerCursor.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
