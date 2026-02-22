using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewSceneLoader : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject mainMenu;
    public GameObject shipLoadingScreen;
    public GameObject finalLoadingScreen;

    [Header("Tips")]
    public GameObject[] tipTexts;
    public float tipDuration = 3f;

    [Header("Image Cycle")]
    public GameObject[] loadingImages;

    [Header("Fade")]
    public CanvasGroup fadePanel;
    public float fadeSpeed = 1.0f;

    [Header("MainGame")]
    public string mainGameSceneName;

    public void StartNewGame()
    {
        
        StartCoroutine(LoadGameSequence());
    }

    IEnumerator LoadGameSequence()
    {
        yield return StartCoroutine(Fade(0,1));

        mainMenu.SetActive(false);

        shipLoadingScreen.SetActive(true);
        yield return StartCoroutine(Fade(1, 0));
        for (int i = 0; i < tipTexts.Length; i++)
        {
            tipTexts[i].SetActive(true);
            if (i > 0)
            {
                tipTexts[i - 1].SetActive(false);
            }

            if (loadingImages.Length > i)
            {
                loadingImages[i].SetActive(true);
            }

            if (i > 0 && loadingImages.Length > i - 1)
            {
                loadingImages[i - 1].SetActive(false);
            }
            yield return new WaitForSeconds(tipDuration);
        }

        if (tipTexts.Length > 0)
        {
            tipTexts[tipTexts.Length -1].SetActive(false);
        }

        if (loadingImages.Length > 0)
        {
            loadingImages[loadingImages.Length - 1].SetActive(false);
        }

        yield return StartCoroutine(Fade(0, 1));

        shipLoadingScreen.SetActive(false);
        finalLoadingScreen.SetActive(true);
        yield return StartCoroutine(Fade(1, 0));

        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(Fade(0, 1));

        SceneManager.LoadScene(mainGameSceneName);
    }

    IEnumerator Fade(float start, float end)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            fadePanel.alpha = Mathf.Lerp(start, end, t);
            yield return null;
        }
    }

    
}
