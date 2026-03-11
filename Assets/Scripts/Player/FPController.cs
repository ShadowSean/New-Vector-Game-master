using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
[RequireComponent (typeof(PlayerInput))]

public class FPController : MonoBehaviour
{
    [Header("Movement")]
    public float walkingSpeed = 5.0f;
    public float runningSpeed = 10.0f;


    [Header("Camera")]
    public Camera playerCam;
    
    public float lookXLimit = 45.0f;
    public float mouseLookSpeed = 0.15f;
    public float controllerLookSpeed = 120f;

    private float defaultMouseLookSpeed;
    private float defaultLookXLimit;
    private float defaultControllerLookSpeed;
    

   


    CharacterController controller;
    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction sprintAction;
    Vector3 moveDir = Vector3.zero;
    float rotationX = 0;

   

    

    [HideInInspector]
    public bool canMove = true;

    private Stamina stamina;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        stamina = FindFirstObjectByType<Stamina>();

        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Camera Look"];

        sprintAction = playerInput.actions["Sprint"];

        defaultMouseLookSpeed = mouseLookSpeed;
        defaultControllerLookSpeed = controllerLookSpeed;
        defaultLookXLimit = lookXLimit;
    }

    private void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void DisableLook()
    {
        mouseLookSpeed = 0;
        controllerLookSpeed = 0;
        lookXLimit = 0;
    }

    public void RestoreLook()
    {
        mouseLookSpeed = defaultMouseLookSpeed;
        controllerLookSpeed = defaultControllerLookSpeed;
        lookXLimit = defaultLookXLimit;
    }
    
    public void SetLookLimits(float newLimit)
    {
        lookXLimit = newLimit;
    }

    public void SetLookSpeeds(float newMouseSpeed, float newControllerSpeed)
    {
        mouseLookSpeed = newMouseSpeed;
        controllerLookSpeed = newControllerSpeed;
    }


    private void Update()
    {
        Vector2 moveInput =  moveAction.ReadValue<Vector2>();
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right   = transform.TransformDirection(Vector3.right);

        bool hasStamina = stamina != null && stamina.hasStamina();
        bool isRunningInput = sprintAction != null && sprintAction.IsPressed() && hasStamina;
        

        float targetSpeed = isRunningInput ? runningSpeed : walkingSpeed;

        Vector3 targetDir = (forward * moveInput.y) + (right * moveInput.x);

        if (targetDir.magnitude > 1f)
            targetDir.Normalize();
        
        moveDir = targetDir * targetSpeed;

        controller.Move(moveDir * Time.deltaTime);

        if (!canMove) return;

        float lookX = lookInput.x;
        float lookY = lookInput.y;

        bool usingGamepad = Gamepad.current != null && Gamepad.current.rightStick.ReadValue().sqrMagnitude > 0.001f;


        if (usingGamepad)
        {
            rotationX += -lookY * controllerLookSpeed * Time.deltaTime;
            transform.Rotate(0f, lookX * controllerLookSpeed * Time.deltaTime, 0f);
        }
        else
        {
            rotationX += -lookY * mouseLookSpeed;
            transform.Rotate(0f, lookX * mouseLookSpeed, 0f);
        }

        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);




    }

}
