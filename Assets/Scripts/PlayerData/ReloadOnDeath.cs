using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadOnDeath : MonoBehaviour
{
    public static ReloadOnDeath instance;
    bool pendinLoad;

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
        pendinLoad = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(!pendinLoad)
        {
            return;
        }

        pendinLoad = false;

        var handler = FindFirstObjectByType<GameHandler>(); 
        if(handler != null)
        {
            handler.Load();
        }
    }
}
