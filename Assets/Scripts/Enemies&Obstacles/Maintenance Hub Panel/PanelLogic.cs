using UnityEngine;
using UnityEngine.UI;

public class PanelLogic : MonoBehaviour
{
    [Header("Generator UI")]
    public GameObject repairAndGenerator;
    public Slider repairPercentage;
    public Button closeGeneratorUI;

    [Header("Base Settings")]
    public GameObject playerCursor;
    public GameObject escapebayDoor;
    public float repairSpeed = 0.5f;
    

    

    bool inRange;
    public static bool isPanelFixed;
  

    private FPController movement;


    private void Start()
    {
        movement = FindFirstObjectByType<FPController>();
        closeGeneratorUI.onClick.AddListener(CloseCodeUI);

        repairAndGenerator.SetActive(false);
        
        repairPercentage.gameObject.SetActive(false);

       
    }

    private void Update()
    {
        if (inRange)
        {
            if (Input.GetMouseButton(0))
            {
                if (movement != null)
                {
                    movement.canMove = false;
                }
                repairPercentage.value += repairSpeed * Time.deltaTime;

                GetComponent<Collider>().enabled = false;

                if (repairPercentage.value >= repairPercentage.maxValue)
                {

                    repairPercentage.value = repairPercentage.maxValue;
                    isPanelFixed = true;

                    escapebayDoor.SetActive(false);

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    

                }
            }

        }       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            playerCursor.SetActive(false);
            inRange = true;
            repairAndGenerator.SetActive(true);
            repairPercentage.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            playerCursor.SetActive(true);
            inRange = false;
            repairAndGenerator.SetActive(false);
            repairPercentage.gameObject.SetActive(false);
           

            
        }
    }

    

    void CloseCodeUI()
    {
        CloseUI(repairAndGenerator);
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
