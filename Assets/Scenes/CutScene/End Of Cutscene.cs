using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public class EndOfCutscene : MonoBehaviour
{
    public PlayableDirector director;

    void Start()
    {
        director.stopped += OnTimelineEnd;
    }

    void OnTimelineEnd(PlayableDirector pd)
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    void OnDestroy()
    {
        if (director != null)
            director.stopped -= OnTimelineEnd;
    }
}
