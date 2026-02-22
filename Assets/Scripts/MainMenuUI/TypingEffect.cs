using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypingEffect : MonoBehaviour
{
    public string gameOverText;

    public float typeSpeed = 0.05f;

    Text mainText;

    private void Awake()
    {
        mainText = GetComponent<Text>();
    }

    private void OnEnable()
    {
        StartCoroutine(TypingText());
    }

    IEnumerator TypingText()
    {
        mainText.text = "";

        foreach (char c in gameOverText)
        {
            mainText.text += c;

            yield return new WaitForSeconds(typeSpeed);
        }
    }
}
