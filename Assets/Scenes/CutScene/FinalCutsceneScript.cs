using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public class FinalCutsceneScript : MonoBehaviour
{
    public PlayableDirector director;

    void Start()
    {
        director.stopped += OnTimelineEnd;
    }

    void OnTimelineEnd(PlayableDirector pd)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }

    void OnDestroy()
    {
        if (director != null)
            director.stopped -= OnTimelineEnd;
    }
}
