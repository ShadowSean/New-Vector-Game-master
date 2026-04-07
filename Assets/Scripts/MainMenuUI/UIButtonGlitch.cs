using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIButtonGlitch : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler
{
    public RectTransform rectTransform;
    public Image image;
    public TMP_Text text;
    public CanvasGroup canvasGroup;

    //Glitch Settings
    public float glitchDuration = 0.15f;

    //Chromatic Flash Settings
    public float chromaticDuration = 0.08f;
    public float chromaticOffset = 2f;

    private Vector3 originalScale;
    private Vector2 originalPos;
    private Color originalColor;
    private string originalText;

    private bool isFlashing = false;

    void Start()
    {
        originalScale = rectTransform.localScale;
        originalPos = rectTransform.anchoredPosition;

        if (image != null)
            originalColor = image.color;

        if (text != null)
            originalText = text.text;
    }

    //Hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(GlitchEffect());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();

        ResetVisuals();
    }

    //Selected
    public void OnPointerClick(PointerEventData eventData)
    {
        TriggerChromatic();
    }

    public void OnSelect(BaseEventData eventData)
    {
        TriggerChromatic();
    }

    void TriggerChromatic()
    {
        if (!isFlashing)
            StartCoroutine(ChromaticFlash());
    }

    IEnumerator ChromaticFlash()
    {
        isFlashing = true;

        float time = 0f;

        while (time < chromaticDuration)
        {
            //Small jitter
            rectTransform.anchoredPosition = originalPos + Random.insideUnitCircle * chromaticOffset;

            //Color shift
            if (image != null)
            {
                image.color = new Color(
                    originalColor.r + Random.Range(0f, 0.2f),
                    originalColor.g,
                    originalColor.b + Random.Range(0f, 0.2f),
                    originalColor.a
                );
            }

            time += Time.deltaTime;
            yield return null;
        }

        ResetVisuals();
        isFlashing = false;
    }

    IEnumerator GlitchEffect()
    {
        float time = 0f;

        while (time < glitchDuration)
        {
            rectTransform.localScale = originalScale * 1.05f;
            rectTransform.anchoredPosition = originalPos + Random.insideUnitCircle * 1.5f;

            if (image != null)
            {
                image.color = originalColor + new Color(
                    Random.Range(-0.1f, 0.1f),
                    Random.Range(-0.1f, 0.1f),
                    Random.Range(-0.1f, 0.1f),
                    0
                );
            }

            if (text != null)
            {
                text.text = GlitchText(originalText);
            }

            canvasGroup.alpha = Random.Range(0.6f, 1f);

            time += 0.03f;
            yield return new WaitForSeconds(0.03f);
        }

        ResetVisuals();
    }

    void ResetVisuals()
    {
        rectTransform.localScale = originalScale;
        rectTransform.anchoredPosition = originalPos;
        canvasGroup.alpha = 1f;

        if (image != null)
            image.color = originalColor;

        if (text != null)
            text.text = originalText;
    }

    string GlitchText(string input)
    {
        char[] chars = input.ToCharArray();

        for (int i = 0; i < chars.Length; i++)
        {
            if (Random.value > 0.8f)
                chars[i] = (char)Random.Range(33, 126);
        }

        return new string(chars);
    }
}

