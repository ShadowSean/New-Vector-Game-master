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

    public void NormalDifficulty()
    {
        selectDiffculty = Difficulty.Normal;
    }
    
    public void HardDifficulty()
    {
        selectDiffculty = Difficulty.Hard;
    }

    public void InsaneDifficulty()
    {
        selectDiffculty = Difficulty.Insane;
    }

    public void DeleteSave(string key)
    {
        if (!PlayerPrefs.HasKey("SaveSlot"))
        {
            Debug.Log("Save Slot does not exist");
            return;
        }

        PlayerPrefs.DeleteKey("SaveSlot");
        Debug.Log(key + "save has been deleted");



    }
}
