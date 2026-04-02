using System.Collections;
using UnityEngine;
using TMPro;

public class PopupUI : MonoBehaviour
{
    public static PopupUI Instance { get; private set; }

    [Header("UI References")]
    public TMP_Text popupText;

    private Coroutine _hideCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (popupText != null)
            popupText.gameObject.SetActive(false);
    }

    public void Show(string message, float duration = 2.5f)
    {
        if (popupText == null) return;

        if (_hideCoroutine != null)
            StopCoroutine(_hideCoroutine);

        popupText.text = message;
        popupText.gameObject.SetActive(true);
        _hideCoroutine = StartCoroutine(HideAfterDelay(duration));
    }

    public void Hide()
    {
        if (_hideCoroutine != null)
            StopCoroutine(_hideCoroutine);

        if (popupText != null)
            popupText.gameObject.SetActive(false);
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        popupText.gameObject.SetActive(false);
    }
}