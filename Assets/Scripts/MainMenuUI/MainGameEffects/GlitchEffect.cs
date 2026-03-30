using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class GlitchEffect : MonoBehaviour
{

    private TextMeshProUGUI glitchText;
    private string defaultText;
    
    void Start()
    {
        glitchText = GetComponent<TextMeshProUGUI>();
        defaultText = glitchText.text;
    }

    IEnumerator StartGlitch()
    {
        int letterIndex = 0;
        while (letterIndex <= defaultText.Length)
        {
            int randomCount = 0;
            while (randomCount < 5)
            {
                glitchText.text = RandomizeText(defaultText, letterIndex);
                yield return new WaitForSeconds(0.02f);
                randomCount++;
            }
            letterIndex++;
        }
    }

    
    

    private string RandomizeText(string text, int startIndex)
    {
        string glitchCharacters = "#&*-+@$103284939`~%^ABCDEFGHIJKLMNOP";
        for (int i = startIndex; i < text.Length; i++)
        {
            StringBuilder sb = new StringBuilder(text);
            sb[i] = glitchCharacters[Random.Range(0,glitchCharacters.Length)];
            text = sb.ToString();
        }
        return text;
    }

    

    private void Update()
    {
       StartCoroutine(StartGlitch());
    }

    
}
