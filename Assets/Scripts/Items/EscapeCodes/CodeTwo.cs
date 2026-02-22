using UnityEngine;
using UnityEngine.UI;

public class CodeTwo : MonoBehaviour
{
    private FPController movement;
    public bool isTwoCollected;

    public GameObject codeTWOUI;

    public Button codeTwoClose;

    private void Start()
    {
        movement = FindFirstObjectByType<FPController>();
        codeTwoClose.onClick.AddListener(CloseCodeUI);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (movement != null)
            {
                movement.canMove = false;
            }

            codeTWOUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isTwoCollected = true;

            GetComponent<Collider>().enabled = false;
        }
    }

    void CloseCodeUI()
    {
        CloseUI(codeTWOUI);
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
