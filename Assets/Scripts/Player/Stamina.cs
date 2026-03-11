using UnityEngine;
using UnityEngine.InputSystem;
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

    private PlayerInput playerInput;
    private InputAction sprintAction;
    private InputAction moveAction;

    private void Awake()
    {
        playerInput = FindFirstObjectByType<PlayerInput>();
        if (playerInput != null)
        {
            sprintAction = playerInput.actions["Sprint"];
            moveAction = playerInput.actions["Move"];
        }
        else
        {
            Debug.LogWarning("Stamina: No playerinput was detected.");
        }
    }

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

        bool isRunning = sprintAction.IsPressed();
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        bool isMoving = moveInput.sqrMagnitude > 0.01f;

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
