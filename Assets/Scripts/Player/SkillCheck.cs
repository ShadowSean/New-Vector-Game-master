using UnityEngine;
using UnityEngine.InputSystem;
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
    float x;
    float y;
    float z;

    private void Awake()
    {
        x = skillCheckArea.transform.rotation.z;
       
        x = Random.Range(0, 360);

        skillCheckArea.transform.Rotate(0,0,x);
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
        x = Mathf.Abs(skillCheckArea.transform.rotation.z * 100);
        y = x + 36;
        z = Mathf.Abs(greenPoint.transform.rotation.z * 100);

        

        if (spaceAction != null && spaceAction.WasPressedThisFrame() && z >= x && z <= y  )
        {
            hasSkill = true;
            Debug.Log("Success");
            


        }
        else if(spaceAction != null && spaceAction.WasPressedThisFrame())
        {
            Debug.Log("You Failed");
            failedSkill = true;
            
            


        }


    }
}
