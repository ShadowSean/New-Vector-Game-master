using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ReloadOnDeath : MonoBehaviour
{
    public static ReloadOnDeath instance;
    bool pendingLoad;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if(instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public void ReloadCurrentSceneCheckpoint()
    {
        pendingLoad = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!pendingLoad)
            return;

        pendingLoad = false;
        StartCoroutine(LoadCheckpointNextFrame());
    }

    private IEnumerator LoadCheckpointNextFrame()
    {
        yield return null; // wait 1 frame so Start() methods finish

        var handler = FindFirstObjectByType<GameHandler>();
        if (handler != null)
        {
            handler.Load();
        }
        else
        {
            Debug.LogWarning("GameHandler not found after scene reload.");
        }
    }
}
