using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

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
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }
}
