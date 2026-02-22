using UnityEngine;

public class CrateTwoUI : MonoBehaviour
{
    public GameObject crateui, equipIcon, playerCursor;

    public static bool partsCollectedTwo;

    bool inRange;
    bool itemEquipped;
    
    public GameObject itemRotation;

    private FPController cameraMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraMovement = other.GetComponent<FPController>();
            if (cameraMovement != null)
            {
                cameraMovement.lookXLimit = 0;
                cameraMovement.LookSpeed = 0;
            }
            inRange = true;
            playerCursor.SetActive(false);
            itemRotation.SetActive(true);
            crateui.SetActive(true);
            equipIcon.SetActive(true);
          
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (inRange && other.CompareTag("Player"))
        {
            if (Input.GetMouseButtonDown(0) && !itemEquipped)
            {
                if (cameraMovement != null)
                {
                    cameraMovement.lookXLimit = 40;
                    cameraMovement.LookSpeed = 5;
                }
                playerCursor.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                equipIcon.SetActive(false);
                itemRotation.SetActive(false);

                itemEquipped = true;
                partsCollectedTwo = true;
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
                cameraMovement.lookXLimit = 45;
                cameraMovement.LookSpeed = 5;
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
