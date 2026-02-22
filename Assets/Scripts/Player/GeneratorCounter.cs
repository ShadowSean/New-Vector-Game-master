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

    public void AddGenerator()
    {
        partCount++;
        counterText.text = partCount.ToString();

        if (partCount >= totalGens)
        {
            TurnTextsGreen();
        }
    }

    void TurnTextsGreen()
    {
        foreach (var text in greenTexts)
        {
            text.color = Color.green;
        }
    }
}
