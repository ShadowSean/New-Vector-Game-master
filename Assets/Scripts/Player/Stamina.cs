using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    public Image staminaImage;       // ← assign your UI Image
    public float maxStamina = 100f;
    public float staminaDrain = 10f;
    public float staminaRegen = 5f;

    [HideInInspector] public float currentStam;
    private FPController staminaMovement;

    private void Start()
    {
        staminaMovement = FindFirstObjectByType<FPController>();
        currentStam = maxStamina;

        if (staminaImage != null)
        {
            staminaImage.fillAmount = 1f;  // full stamina
        }
    }

    private void Update()
    {
        if (staminaMovement == null) return;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

        if (isRunning && isMoving && currentStam > 0)
        {
            currentStam -= staminaDrain * Time.deltaTime;
            if (currentStam < 0) currentStam = 0;
        }
        else
        {
            if (currentStam < maxStamina)
                currentStam += staminaRegen * Time.deltaTime;
        }

        // Update UI Image Fill
        if (staminaImage != null)
        {
            staminaImage.fillAmount = currentStam / maxStamina;
        }
    }

    public bool hasStamina()
    {
        return currentStam > 0;
    }
}
