using UnityEngine;
using UnityEngine.UI;

public class Elevator : MonoBehaviour
{
    public GameObject firstFloor, secondFloor,player, elevatorUI,playerScope;
    public Button firstUI, secondUI;
    private FPController moveMent;

    


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform.root.gameObject;
            moveMent = player.GetComponent<FPController>();
            if (moveMent != null)
            {
                moveMent.lookXLimit = 0;
                moveMent.LookSpeed = 0;
            }
            
            playerScope.SetActive(false);
            elevatorUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Button firstButton = firstUI.GetComponent<Button>();
            Button secondButton = secondUI.GetComponent<Button>();

            if (firstButton != null)
            {
                firstButton.onClick.AddListener(() => GoToFloor(firstFloor.transform.position));
            }
            if (secondButton != null)
            {
                secondButton.onClick.AddListener(() => GoToFloor(secondFloor.transform.position));
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (moveMent != null)
            {
                moveMent.lookXLimit = 45;
                moveMent.LookSpeed = 5;
            }
            
            playerScope.SetActive(true);
            elevatorUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void GoToFloor(Vector3 targetPosition)
    {
        Debug.Log("Button clicked! Teleporting player...");
        if (player != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false;

                RaycastHit hit;
                Vector3 adjustedPosition = targetPosition;
                if(Physics.Raycast(targetPosition + Vector3.up * 2,Vector3.down,out hit, 10f)) 
                {
                    adjustedPosition = hit.point + Vector3.up * (cc.height / 2f);
                }
                player.transform.position = adjustedPosition;
                cc.enabled = true;
            }
            else
            {
                player.transform.position = targetPosition;
            }
            
            elevatorUI.SetActive(false);
            playerScope.SetActive(true);
            
            Cursor.lockState= CursorLockMode.Locked;
            Cursor.visible = false;

            if (moveMent != null)
            {
                moveMent.lookXLimit = 45;
                moveMent.LookSpeed = 5;
            }
        }
        else
        {
            Debug.LogWarning("Player is null! No teleport performed.");
        }
    }
}
