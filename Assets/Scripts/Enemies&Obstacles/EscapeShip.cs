using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EscapeShip : MonoBehaviour
{

    [Header("Player Settings")]
    public GameObject inventory;
    public GameObject staminaAndItem;
    public GameObject scope;


    public string sceneName = "Final Cutscene";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(GeneratorSequence());
        }
    }

    private IEnumerator GeneratorSequence()
    {

        if (inventory) inventory.SetActive(false);
        if (staminaAndItem) staminaAndItem.SetActive(false);
        if (scope) scope.SetActive(false);

        

        yield return new WaitForSeconds(0.5f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(sceneName);
    }
}
