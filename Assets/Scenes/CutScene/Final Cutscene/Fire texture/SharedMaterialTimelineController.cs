using UnityEngine;

public class SharedMaterialTimelineController : MonoBehaviour
{
    [Header("The material shared by all your objects")]
    public Material sharedMat;

    [Header("Animate these in Timeline")]
   
    [Range(0f, 10f)] public float emissionStrength = 1f;
    public Color emissionColor = Color.red;

    void LateUpdate()
    {
        if (sharedMat == null) return;

       
     
        sharedMat.SetColor("_EmissionColor", emissionColor * 3f * emissionStrength);
    }
}