using UnityEngine;

public enum Difficulty
{
    Normal,
    Hard,
    Insane
}
public class DifficultyChanger1 : MonoBehaviour
{
    

    public static Difficulty selectDiffculty = Difficulty.Normal;

    private string GetSaveKey()
    {
        return "SaveSlot_" + selectDiffculty.ToString();
    }

    public void NormalDifficulty()
    {
        selectDiffculty = Difficulty.Normal;
        PlayerPrefs.SetInt("SelectedDifficulty", (int)selectDiffculty);
        PlayerPrefs.Save();
        DeleteSave();
    }
    
    public void HardDifficulty()
    {
        selectDiffculty = Difficulty.Hard;
        PlayerPrefs.SetInt("SelectedDifficulty", (int)selectDiffculty);
        PlayerPrefs.Save();
        DeleteSave();
    }

    public void InsaneDifficulty()
    {
        selectDiffculty = Difficulty.Insane;
        PlayerPrefs.SetInt("SelectedDifficulty", (int)selectDiffculty);
        PlayerPrefs.Save();
        DeleteSave();
    }

    public void DeleteSave()
    {
        string saveKey = GetSaveKey();

        if (PlayerPrefs.HasKey(saveKey))
        {
            PlayerPrefs.DeleteKey(saveKey);
            PlayerPrefs.Save();
            Debug.Log(saveKey + "save has been deleted");
        }
    }

    private void Awake()
    {
        if (PlayerPrefs.HasKey("SelectedDifficulty"))
        {
            selectDiffculty = (Difficulty)PlayerPrefs.GetInt("SelectedDifficulty");
        }
    }
}
