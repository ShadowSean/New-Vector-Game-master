using UnityEngine;
using UnityEngine.UI;

public class CodeOne : MonoBehaviour
{
    private FPController movement;
    public bool isOneCollected;

    public GameObject codeoneUI;

    public Button codeOneClose;

    private void Start()
    {
        movement= FindFirstObjectByType<FPController>();    
        codeOneClose.onClick.AddListener(CloseCodeUI);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (movement != null)
            {
                movement.canMove = false;
            }

            codeoneUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isOneCollected = true;

            GetComponent<Collider>().enabled = false;
        }
    }

    void CloseCodeUI()
    {
        CloseUI(codeoneUI);
    }

    void CloseUI(GameObject UI)
    {
        UI.SetActive(false);
        if (movement != null)
        {
            movement.canMove = true;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
