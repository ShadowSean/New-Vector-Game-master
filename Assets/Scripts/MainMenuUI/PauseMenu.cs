using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameisPaused;

    public GameObject pauseMenuUI;

    public GameObject controlsMenu;

    public string MainMenu;

    public GameObject playerCursor;

    private FPController movementAndRotation;

    private PlayerInput playerInput;

    private InputAction menuToggleAction;

    private void Awake()
    {
        playerInput = FindFirstObjectByType<PlayerInput>();

        if (playerInput != null)
        {
            menuToggleAction = playerInput.actions["Menu Toggle"];
        }
        else
        {
            Debug.LogWarning("PauseMenu: No player input was detected.");
        }
    }


    private void Start()
    {
        movementAndRotation = FindFirstObjectByType<FPController>();
    }


    private void Update()
    {
        if (menuToggleAction != null && menuToggleAction.WasPressedThisFrame())
        {
            if (gameisPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        if (movementAndRotation != null)
        {
            movementAndRotation.canMove = true;
        }
        playerCursor.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameisPaused = false;
    }

    public void ControlsMenu()
    {
        controlsMenu.SetActive(true);
    }

   public void Pause()
    {
        if (movementAndRotation != null)
        {

            movementAndRotation.canMove = false;
        }
        playerCursor.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameisPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenu);
    }

}
