using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [Header("Player Position and Rotation")]
    [SerializeField] Transform posAndRot;
    public Transform defaultPos;


    [Header("Number of Gens Fixed")]
    [SerializeField] GeneratorCounter generatorCounter;

    [Header("Number of batteries collected")]
    [SerializeField] FasterGen batteryCounter;

    [Header("Spare Parts Collected")]
    [SerializeField] SparePartsCounter partsCollected;

    [Header("Store items")]
    [SerializeField] ItemSwitcher hasItems;

    [Header("Crate Scripts")]
    [SerializeField] CrateUI crateOne;
    [SerializeField] CrateTwoUI crateTwo;
    [SerializeField] CrateThreeUI crateThree;
    [SerializeField] CrateFourUI crateFour;
    [SerializeField] CrateFiveUI crateFive;
    [SerializeField] CrateSixUI crateSix;

    [Header("Pickup Items")]
    [SerializeField] private pickupItem[] pickups;

    [Header("Generator Saves")]
    [SerializeField] GeneratorLogic generatorOne;
    [SerializeField] SecondGeneratorLogic generatorTwo;
    [SerializeField] GeneratorThree generatorThree;
    [SerializeField] GeneratorFour generatorFour;
    [SerializeField] GeneratorFive generatorFive;
    [SerializeField] GeneratorSix generatorSix;

    [Header("Vent")]
    [SerializeField] VentLogic ventLogic;


    public string GetSaveKey()
    {
        return "SaveSlot_" + DifficultyChanger1.selectDiffculty.ToString();
    }

    public void Save()
    {
        string saveKey = GetSaveKey();

        bool[] pickupStates = new bool[pickups.Length];
        for (int i = 0; i < pickups.Length; i++)
        {
            if (pickups[i] != null)
            {
                pickupStates[i] = pickups[i].GetCollectedState();
            }
        }

        Debug.Log("Save data function called.");
        SaveData saveData = new SaveData {
            playerPosition = posAndRot.position,
            playerRotation = posAndRot.rotation,
            generatorsRepaired = generatorCounter.partCount,
            batteriesCollected = batteryCounter.batteryCount,
            sparepartsCollected = partsCollected.partCount,
            isFlashlightCollected = hasItems.hasFlashlight,
            isTaserCollected = hasItems.hasTaser,
            isFlameCollected = hasItems.hasFlamethrower,
            crateDisabledAfterClaim =  crateOne != null && crateOne.GetCrateDisabledState(),
            crateDisabledAfterClaimTwo = crateTwo != null && crateTwo.GetCrateDisabledState(),
            crateDisabledAfterClaimThree = crateThree != null && crateThree.GetCrateDisabledState(),
            crateDisabledAfterClaimFour = crateFour != null && crateFour.GetCrateDisabledState(),
            crateDisabledAfterClaimFive = crateFive != null && crateFive.GetCrateDisabledState(),
            crateDisabledAfterClaimSix = crateSix != null && crateSix.GetCrateDisabledState(),
            pickupCollectedStates = pickupStates,
            genoneFixed = generatorOne != null && generatorOne.GetFixedState(),
            gentwoFixed = generatorTwo != null && generatorTwo.GetFixedState(),
            genthreeFixed = generatorThree != null && generatorThree.GetFixedState(),
            genfourFixed = generatorFour != null && generatorFour.GetFixedState(),
            genfiveFixed = generatorFive != null && generatorFive.GetFixedState(),
            gensixFixed = generatorSix != null && generatorSix.GetFixedState(),
            ventUnlocked = ventLogic != null && ventLogic.GetUnlockedState(),
        };
        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(saveKey,json);
        PlayerPrefs.Save();
        Debug.Log("Data has been saved!");
        Debug.Log("Saved JSON for key " + saveKey + " = " + PlayerPrefs.GetString(saveKey, "NOT_FOUND"));
    }

    public void Load()
    {
        string saveKey = GetSaveKey();

        if (!PlayerPrefs.HasKey(saveKey))
        {
            Debug.Log("Save Slot does not exist for current difficulty " + saveKey);
            //posAndRot.position = defaultPos.position;
            LoadDefaultSpawn();
            return;
        }

        string json = PlayerPrefs.GetString(saveKey);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        CharacterController controller = posAndRot.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        posAndRot.rotation = data.playerRotation;
        posAndRot.position = data.playerPosition;


        if (controller != null)
        {
            controller.enabled = true;
        }

        generatorCounter.partCount = data.generatorsRepaired;
        generatorCounter.RefreshUI();

        batteryCounter.batteryCount = data.batteriesCollected;

        partsCollected.partCount = data.sparepartsCollected;
        partsCollected.RefreshUI();


        if (data.isFlashlightCollected)
        {
            hasItems.PickupFlashlight();
        }

        if (data.isTaserCollected)
        {
            hasItems.PickupTaser();
        }

        if (data.isFlameCollected)
        {
            hasItems.PickupFlamethrower();
        }

        if (crateOne != null)
        {
            crateOne.SetCrateDisabledState(data.crateDisabledAfterClaim);
        }

        if (crateTwo != null)
        {
            crateTwo.SetCrateDisabledState(data.crateDisabledAfterClaimTwo);
        }

        if (crateThree != null)
        {
            crateThree.SetCrateDisabledState(data.crateDisabledAfterClaimThree);
        }

        if (crateFour != null)
        {
            crateFour.SetCrateDisabledState(data.crateDisabledAfterClaimFour);
        }

        if (crateFive != null)
        {
            crateFive.SetCrateDisabledState(data.crateDisabledAfterClaimFive);
        }

        if (crateSix != null)
        {
            crateSix.SetCrateDisabledState(data.crateDisabledAfterClaimSix);
        }

        if(generatorOne != null)
        {
            generatorOne.LoadGeneratorState(data.genoneFixed);
        }

        if (generatorTwo != null)
        {
            generatorTwo.LoadGeneratorState(data.gentwoFixed);
        }

        if (generatorThree != null)
        {
            generatorThree.LoadGeneratorState(data.genthreeFixed);
        }

        if (generatorFour != null)
        {
            generatorFour.LoadGeneratorState(data.genfourFixed);
        }

        if (generatorFive != null)
        {
            generatorFive.LoadGeneratorState(data.genfiveFixed);
        }

        if (generatorSix != null)
        {
            generatorSix.LoadGeneratorState(data.gensixFixed);
        }

        if (data.pickupCollectedStates != null)
        {
            for (int i = 0; i < pickups.Length && i < data.pickupCollectedStates.Length; i++)
            {
                if (pickups[i] != null)
                {
                    pickups[i].SetCollectedState(data.pickupCollectedStates[i]);
                }
            }
        }

        if (ventLogic != null)
        {
            ventLogic.LoadVentState(data.ventUnlocked);
        }

        Debug.Log("Data is now loaded for difficulty: " + DifficultyChanger1.selectDiffculty);
    }

    
    void LoadDefaultSpawn()
    {
        CharacterController controller = posAndRot.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        generatorCounter.partCount = 0;
        generatorCounter.RefreshUI();

        partsCollected.partCount = 0;
        partsCollected.RefreshUI();

        posAndRot.position = defaultPos.position;
        posAndRot.rotation = defaultPos.rotation;

        if (controller != null)
        {
            controller.enabled = true;
        }
        if (crateOne != null) crateOne.SetCrateDisabledState(false);
        if (crateTwo != null) crateTwo.SetCrateDisabledState(false);
        if (crateThree != null) crateThree.SetCrateDisabledState(false);
        if (crateFour != null) crateFour.SetCrateDisabledState(false);
        if (crateFive != null) crateFive.SetCrateDisabledState(false);
        if (crateSix != null) crateSix.SetCrateDisabledState(false);

        if (ventLogic != null) ventLogic.LoadVentState(false);

        Debug.Log("Spawned at defauly position since no save slot exists for this gamemode.");
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
    public bool crateDisabledAfterClaim;
    public bool crateDisabledAfterClaimTwo;
    public bool crateDisabledAfterClaimThree;
    public bool crateDisabledAfterClaimFour;
    public bool crateDisabledAfterClaimFive;
    public bool crateDisabledAfterClaimSix;
    public bool[] pickupCollectedStates;
    public bool ventUnlocked;


    public bool genoneFixed;
    public bool gentwoFixed;
    public bool genthreeFixed;
    public bool genfourFixed;
    public bool genfiveFixed;
    public bool gensixFixed;
}



