using UnityEngine;

public class CameraSway : MonoBehaviour
{
    [Header("Camera Sway Settings")]
    public bool canSway = true;
    public GameObject camPivot;
    public float zStart = 1.0f;
    public float zEnd = -1.0f;
    public float zMoveSpeed = 0.5f;
    
    private float SwayTimer = 0f;

    private void Update()
    {
        if (!canSway) return;

        SwayTimer += Time.deltaTime * zMoveSpeed;

        float t = Mathf.PingPong(SwayTimer, 1f);
        float sway = Mathf.Lerp(zStart, zEnd, t);

        Vector3 currentPivotEuler = camPivot.transform.localEulerAngles;
        currentPivotEuler.z = sway;
        camPivot.transform.localEulerAngles = currentPivotEuler;
        
    }



}
