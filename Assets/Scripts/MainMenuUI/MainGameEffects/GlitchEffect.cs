using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class GlitchEffect : MonoBehaviour
{

    private TextMeshProUGUI glitchText;
    private string defaultText;
    public bool isGlitching;
    
    void Start()
    {
        glitchText = GetComponent<TextMeshProUGUI>();
        defaultText = glitchText.text;
        StartCoroutine(StartGlitch());
    }

    IEnumerator StartGlitch()
    {
        isGlitching = true;
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
        glitchText.text = defaultText;
        isGlitching = false;
        
    }

    
    

    private string RandomizeText(string text, int startIndex)
    {
        
        string glitchCharacters = "#&*-+@$103284939`~%^ABCDEFGHIJKLMNOP";
        StringBuilder sb = new StringBuilder(text);
        for (int i = startIndex; i < text.Length; i++)
        {
            
            sb[i] = glitchCharacters[Random.Range(0,glitchCharacters.Length)];
            
        }
        return sb.ToString();
    }

    

    

    
}
