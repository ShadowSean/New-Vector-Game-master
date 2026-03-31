using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FirstSelectedButton : MonoBehaviour
{
    [SerializeField] GameObject firstSelectedButton;
    private void OnEnable()
    {
        StartCoroutine(SelectButton());
    }

    IEnumerator SelectButton()
    {
        yield return null;
        yield return null;

        Button btn = firstSelectedButton.GetComponent<Button>();
        if (btn != null && btn.interactable)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
       
       
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }
}
