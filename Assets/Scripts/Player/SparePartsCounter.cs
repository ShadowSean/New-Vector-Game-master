using TMPro;
using UnityEngine;

public class SparePartsCounter : MonoBehaviour
{
    public static SparePartsCounter Instance;

    public TextMeshProUGUI counterText;
    public int totalParts = 6;
    public int partCount = 0;
    public TextMeshProUGUI[] greenTexts;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RefreshUI();
    }

    public void AddPart()
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

        if (partCount >= totalParts)
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
            text.color = Color.green;
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
