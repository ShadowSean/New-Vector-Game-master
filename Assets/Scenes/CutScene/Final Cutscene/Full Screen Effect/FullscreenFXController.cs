using UnityEngine;

public class FullscreenFXController : MonoBehaviour
{
    [Header("Material (same as Fullscreen Pass)")]
    public Material fullscreenMaterial;

    [Header("Surface Inputs (Animate these in Timeline)")]

    [Range(0f, 200f)]
    public float noiseScale = 100f;

    [Range(0f, 1f)]
    public float blend = 0f;

    public Vector2 panSpeed = new Vector2(1f, 1f);

    [Range(0f, 5f)]
    public float amplitude = 1f;

    void LateUpdate()
    {
        if (fullscreenMaterial == null) return;

        // IMPORTANT: names must match Shader Graph EXACTLY
        fullscreenMaterial.SetFloat("_NoiseScale", noiseScale);
        fullscreenMaterial.SetFloat("_Blend", blend);
        fullscreenMaterial.SetVector("_PanSpeed", panSpeed);
        fullscreenMaterial.SetFloat("_Amplitude", amplitude);
    }
}