using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [Header("Player Position and Rotation")]
    [SerializeField] Transform posAndRot;

    [Header("Number of Gens Fixed")]
    [SerializeField] GeneratorCounter generatorCounter;

    [Header("Number of batteries collected")]
    [SerializeField] FasterGen batteryCounter;

    [Header("Spare Parts Collected")]
    [SerializeField] SparePartsCounter partsCollected;

    [Header("Store items")]
    [SerializeField] ItemSwitcher hasItems;

    


    public void Save()
    {
        

        Debug.Log("Save data function called.");
        SaveData saveData = new SaveData {
            playerPosition = posAndRot.position,
            playerRotation = posAndRot.rotation,
            generatorsRepaired = generatorCounter.partCount,
            batteriesCollected = batteryCounter.batteryCount,
            sparepartsCollected = partsCollected.partCount,
            isFlashlightCollected = hasItems.hasFlashlight,
            isTaserCollected = hasItems.hasTaser,
            isFlameCollected = hasItems.hasFlamethrower
        };
        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("SaveSlot",json);
        PlayerPrefs.Save();
        Debug.Log("Data has been saved!");
        Debug.Log("Saved JSON = " + PlayerPrefs.GetString("SaveSlot", "NOT_FOUND"));
    }
}

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public int generatorsRepaired;
    public int batteriesCollected;
    public int sparepartsCollected;
    public bool isFlashlightCollected;
    public bool isTaserCollected;
    public bool isFlameCollected;
}

