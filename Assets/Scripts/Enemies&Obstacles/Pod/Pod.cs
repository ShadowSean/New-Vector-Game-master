using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Pod : MonoBehaviour
{
    [Header("Escape Win Settings")]
    public CanvasGroup escapePodCanvas;    
    public float fadeDuration = 2f;       

    [Header("Player Settings")]
    public GameObject inventory;
    public GameObject staminaAndItem;
    public GameObject scope;

    private bool escapePodWin = false;

    public string sceneName = "MainMenu";

    private void OnTriggerEnter(Collider other)
    {
        if (escapePodWin) return;
        if (other.CompareTag("Player"))
        {
            escapePodWin = true;
            StartCoroutine(EscapePodSequence());
        }
    }

    private IEnumerator EscapePodSequence()
    {
        
        if (inventory) inventory.SetActive(false);
        if (staminaAndItem) staminaAndItem.SetActive(false);
        if (scope) scope.SetActive(false);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            if (escapePodCanvas)
                escapePodCanvas.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(sceneName);
    }
}
