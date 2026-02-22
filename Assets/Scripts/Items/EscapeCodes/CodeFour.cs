using UnityEngine;
using UnityEngine.UI;

public class CodeFour : MonoBehaviour
{
    private FPController movement;
    public bool isFourCollected;

    public GameObject codefourUI;

    public Button codeFourClose;

    private void Start()
    {
        movement = FindFirstObjectByType<FPController>();
        codeFourClose.onClick.AddListener(CloseCodeUI);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (movement != null)
            {
                movement.canMove = false;
            }

            codefourUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isFourCollected = true;

            GetComponent<Collider>().enabled = false;
        }
    }

    void CloseCodeUI()
    {
        CloseUI(codefourUI);
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
