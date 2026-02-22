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
}
