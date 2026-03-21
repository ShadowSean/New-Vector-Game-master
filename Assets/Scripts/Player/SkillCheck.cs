using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

public class SkillCheck : MonoBehaviour
{
    public Image greenPoint;
    public Image skillCheckArea;
    private InputAction spaceAction;
    private PlayerInput playerInput;
    public GameObject skillCheckObject;
    public bool hasSkill;
    public bool failedSkill;


    public float successZoneSize = 36;
    public float greenAngleOffset = 39;

    private void Awake()
    {
        float randomRotation = Random.Range(0f, 360f);

        skillCheckArea.transform.rotation = Quaternion.Euler(0, 0, randomRotation);




        playerInput = FindFirstObjectByType<PlayerInput>();

        if (playerInput != null)
        {
            spaceAction = playerInput.actions["Skill Check"];
        }
        else
        {
            Debug.LogWarning("Skill Check: Player input has not been found.");
        }
    }



    private void Update()
    {




        if (spaceAction != null && spaceAction.WasPressedThisFrame())
        {
            if (IsGreenPointInRedZone())
            {
                hasSkill = true;
                Debug.Log("Success");
            }
            else
            {
                failedSkill = true;
                Debug.Log("You Failed");
            }


        }


    }

    private bool IsGreenPointInRedZone()
    {
        float greenAngle = greenPoint.transform.parent.eulerAngles.z + greenAngleOffset;
        float redStarAngle = skillCheckArea.transform.eulerAngles.z;
        float redEndAngle = redStarAngle + successZoneSize;

        greenAngle = (greenAngle % 360f + 360f) % 360f;
        redStarAngle = (redStarAngle % 360f + 360f) % 360f;
        redEndAngle = (redEndAngle % 360f + 360f) % 360f;

        Debug.Log($"Green: {greenAngle:F1} Red: {redStarAngle:F1} -- {redEndAngle:F1}");

        if (redStarAngle <= redEndAngle)
        {
            return greenAngle >= redStarAngle && greenAngle <= redEndAngle;
        }
        else
        {
            return greenAngle >= redStarAngle || greenAngle <= redEndAngle;
        }
    }
}


