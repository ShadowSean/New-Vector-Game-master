using TMPro;
using UnityEngine;

public class GeneratorCounter : MonoBehaviour
{
    public static GeneratorCounter Instance;

    public TextMeshProUGUI counterText;
    public int totalGens = 6;
    public int partCount = 0;
    public TextMeshProUGUI[] greenTexts;
    public int FixedCount
    {
        get { return partCount; }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RefreshUI();
    }

    public void AddGenerator()
    {
        partCount++;
        RefreshUI();
        

        
    }

    public void RefreshUI()
    {
        if (counterText != null)
        {
           counterText.text = partCount.ToString();
        }

        if (partCount >= totalGens)
        {
            TurnTextsGreen();
        }
        else
        {
            ResetTextsColor();
        }
    }

    void TurnTextsGreen()
    {
        foreach (var text in greenTexts)
        {
            if (text != null)
            {
               text.color = Color.green;
            }
            
        }
    }

    void ResetTextsColor()
    {
        foreach (var text in greenTexts)
        {
            if (text != null)
            {
                text.color = Color.white;
            }
        }
    }
}
