using TMPro;
using UnityEngine;
using System.Collections;

public class Vector9TriggerEvent : MonoBehaviour
{
    [Header("Popup Settings")]
    [TextArea(2, 6)]
    [SerializeField] string message = "It's Locked";
    [SerializeField] GameObject popupPrompt;
    [SerializeField] TMP_Text popupText;


    

    private void Awake()
    {
        if (popupText != null)
            popupText.richText = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Vector9Text());
           
            popupText.text = message.Replace("\\n", "\n");

            if (popupPrompt != null) popupPrompt.SetActive(true);
          
        }
        
    }

    

    

    IEnumerator Vector9Text()
    {
        popupPrompt.SetActive(true);
        yield return new WaitForSeconds(5f);
        popupPrompt.SetActive(false);
    }
}
