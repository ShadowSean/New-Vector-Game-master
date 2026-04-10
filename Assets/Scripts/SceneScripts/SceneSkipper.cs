using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneSkipper : MonoBehaviour
{
    public void skiptoshipscene()
    {
        SceneManager.LoadScene("TheShipSectionOne");
    }

    public void skiptomainmenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


}
