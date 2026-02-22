using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EscapeCodes : MonoBehaviour
{
    public GameObject codeOne, codeTwo, codeThree, codeFour, codeFive;

    public GameObject codeUI, codetwoUI, codethreeUI, codefourUI, codefiveUI;

    public Button closeButton, closeButton2, closeButton3, closeButton4, closeButton5;

    bool code1Collected, code2Collected, code3Collected, code4Collected,code5Collected;

    bool isAllCodes;

    private FPController movement;

    private void Start()
    {
        movement = FindFirstObjectByType<FPController>();

        codeUI.SetActive(false);
        codetwoUI.SetActive(false);
        codethreeUI.SetActive(false);
        codefourUI.SetActive(false);
        codefiveUI.SetActive(false);

        closeButton.onClick.AddListener(CloseCodeUI);
        closeButton2.onClick.AddListener(CloseCodeTwoUI);
        closeButton3.onClick.AddListener(CloseCodeThreeUI);
        closeButton4.onClick.AddListener(CloseCodeFourUI);
        closeButton5.onClick.AddListener(CloseCodeFiveUI);
    }

    public void OnPlayerEnteredTrigger(Collider other)
    {
        if (other.gameObject == codeOne && !code1Collected)
        {
            code1Collected = true;
            codeOne.SetActive(false);
            ShowUI(codeUI);
        }
        else if (other.gameObject == codeTwo && !code2Collected)
        {
            code2Collected = true;
            codeTwo.SetActive(false);
            ShowUI(codetwoUI);
        }
        else if (other.gameObject == codeThree && !code3Collected)
        {
            code3Collected = true;
            codeThree.SetActive(false);
            ShowUI(codethreeUI);
        }
        else if (other.gameObject == codeFour && !code4Collected)
        {
            code4Collected = true;
            codeFour.SetActive(false);
            ShowUI(codefourUI);
        }
        else if (other.gameObject == codeFive && !code5Collected)
        {
            code5Collected = true;
            codeFive.SetActive(false);
            ShowUI(codefiveUI);
        }

        CheckAllCodes();
    }

    void ShowUI(GameObject UI)
    {
        UI.SetActive(true);
        if (movement != null)
        {
            movement.canMove = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
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

    void CloseCodeUI()
    {
        CloseUI(codeUI);
    }

    void CloseCodeTwoUI()
    {
        CloseUI(codetwoUI);
    }

    void CloseCodeThreeUI()
    {
        CloseUI(codethreeUI);
    }

    void CloseCodeFourUI()
    {
        CloseUI(codefourUI);
    }

    void CloseCodeFiveUI()
    {
        CloseUI(codefiveUI);
    }

    void CheckAllCodes()
    {
        if (code1Collected && code2Collected && code3Collected && code4Collected && code5Collected)
        {
            isAllCodes = true;
            Debug.Log("All codes have been collected");
        }
    }
}
