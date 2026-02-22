using UnityEngine;
using UnityEngine.UI;

public class CodeFive : MonoBehaviour
{
    private FPController movement;
    public bool isFiveCollected;

    public GameObject codefiveUI;

    public Button codeFiveClose;

    private void Start()
    {
        movement = FindFirstObjectByType<FPController>();
        codeFiveClose.onClick.AddListener(CloseCodeUI);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (movement != null)
            {
                movement.canMove = false;
            }

            codefiveUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isFiveCollected = true;

            GetComponent<Collider>().enabled = false;
        }
    }

    void CloseCodeUI()
    {
        CloseUI(codefiveUI);
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
