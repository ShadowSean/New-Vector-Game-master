using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuTextFade : MonoBehaviour
{
    public float fadeSpeed = 1.0f;

    Text pulsatingText;

    Color pulsatingColor;

    private void Start()
    {
        pulsatingText = GetComponent<Text>();
        pulsatingColor = pulsatingText.color;
    }

    private void Update()
    {
        float alpha = (Mathf.Sin(Time.time * fadeSpeed) + 1f) / 2f;
        pulsatingText.color = new Color(pulsatingColor.r, pulsatingColor.g,pulsatingColor.b,alpha);
    }
}
