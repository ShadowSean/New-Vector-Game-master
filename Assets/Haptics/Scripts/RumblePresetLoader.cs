#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// Run this once to generate all preset assets in Assets/Haptics/Presets/.
public static class RumblePresetLoader
{
#if UNITY_EDITOR
    [MenuItem("Tools/Haptics/Generate Preset Assets")]
    static void GenerateAll()
    {
        string folder = "Assets/Haptics/Presets";
        if (!AssetDatabase.IsValidFolder(folder))
        {
            AssetDatabase.CreateFolder("Assets/Haptics", "Presets");
        }

        Save(RumbleProfile.DistantFall(), folder, "DistantFall");
        Save(RumbleProfile.FloorImpact(), folder, "FloorImpact");
        Save(RumbleProfile.RobotGrab(), folder, "RobotGrab");
        Save(RumbleProfile.RobotStep(), folder, "RobotStep");
        Save(RumbleProfile.Jumpscare(), folder, "Jumpscare");
        Save(RumbleProfile.CryopodOpen(), folder, "CryopodOpen");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[Haptics] Preset assets created in " + folder);
    }

    static void Save(RumbleProfile p, string folder, string name)
    {
        string path = $"{folder}/{name}.asset";
        var existing = AssetDatabase.LoadAssetAtPath<RumbleProfile>(path);
        if (existing == null)
            AssetDatabase.CreateAsset(p, path);
        else
            Debug.Log($"[Haptics] Skipped {name} — already exists.");
    }
#endif
}