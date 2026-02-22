using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EscapeShip : MonoBehaviour
{
    [Header("Generator Win Settings")]
    public CanvasGroup generatorWinCanvas;
    public float fadeDuration = 2f;

    [Header("Player Settings")]
    public GameObject inventory;
    public GameObject staminaAndItem;
    public GameObject scope;

    private bool generatorWin = false;

    public string sceneName = "MainMenu";

    private void OnTriggerEnter(Collider other)
    {
        if (generatorWin) return;
        if (other.CompareTag("Player"))
        {
            generatorWin = true;
            StartCoroutine(GeneratorSequence());
        }
    }

    private IEnumerator GeneratorSequence()
    {

        if (inventory) inventory.SetActive(false);
        if (staminaAndItem) staminaAndItem.SetActive(false);
        if (scope) scope.SetActive(false);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            if (generatorWinCanvas)
                generatorWinCanvas.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(sceneName);
    }
}
