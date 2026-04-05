using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    
    public static InputManager Instance;

    [HideInInspector] public ControllerActions controls;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        controls = new ControllerActions();

    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

}
