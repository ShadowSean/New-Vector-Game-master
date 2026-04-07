using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMiniZoom : MonoBehaviour
{
    [Header("Zoom Settings")]
    public Camera cam;
    public float normalFOV = 40f;
    public float zoomFOV = 20f;
    public float zoomSpeed = 5f;

    private PlayerInput playerInput;
    private InputAction rightclickAction;

    bool isZooming;

    private void Awake()
    {
        playerInput = FindFirstObjectByType<PlayerInput>();

        if (playerInput != null)
        {
            rightclickAction = playerInput.actions["CamZoom"];
        }
    }
    private void Update()
    {
       if(rightclickAction != null && rightclickAction.IsPressed()) {
            isZooming = true;
       }

       if (rightclickAction != null && rightclickAction.WasReleasedThisFrame())
       {
            isZooming = false;
       }

       if (isZooming)
       {
           cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomFOV, zoomSpeed * Time.deltaTime);
       }
       else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFOV, zoomSpeed * Time.deltaTime);

        }
    }
}
