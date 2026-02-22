using UnityEngine;
using UnityEngine.UI;

public class PodCode : MonoBehaviour
{
    [Header("UI")]
    public GameObject podCodeUI;

    [Header("InputText")]
    public InputField escapeCode;
    public string correctCode = "54321";
    public Button closeButton;

    [Header("Pod")]
    public GameObject pod;

    [Header("Player Settings")]
    public GameObject playerScope;

    private FPController movement;

    private void Start()
    {
        movement = FindFirstObjectByType<FPController>();
        closeButton.onClick.AddListener(CloseCodeUI);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (movement != null)
            {
                movement.canMove = false;
            }
            podCodeUI.SetActive(true);
            playerScope.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (escapeCode.text == correctCode)
            {
                pod.SetActive(true);
            }
        }
    }

    void CloseCodeUI()
    {
        CloseUI(podCodeUI);
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
        playerScope.SetActive(true);
    }
}
