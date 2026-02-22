using UnityEngine;
using UnityEngine.UI;

public class CodeThree : MonoBehaviour
{
    private FPController movement;
    public bool isThreeCollected;

    public GameObject codethreeUI;

    public Button codethreeClose;

    private void Start()
    {
        movement = FindFirstObjectByType<FPController>();
        codethreeClose.onClick.AddListener(CloseCodeUI);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (movement != null)
            {
                movement.canMove = false;
            }

            codethreeUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isThreeCollected = true;

            GetComponent<Collider>().enabled = false;
        }
    }

    void CloseCodeUI()
    {
        CloseUI(codethreeUI);
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
