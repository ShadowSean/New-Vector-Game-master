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

    public void AddPart()
    {
        partCount++;
        counterText.text = partCount.ToString();

        if (partCount >= totalParts)
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
