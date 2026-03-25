using UnityEngine;


public class ViewBobbing : MonoBehaviour
{
    [Header("View Bob Settings")]
    public float bobFrequency = 2.5f;
    public float bobHorizontalAmplitude = 0.05f;
    public float bobVerticalAmplitude = 0.05f;

    [Header("Smoothing")]
    public float bobSmoothing = 10f;
    public float returnSmoothing = 6f;

    [Header("Camera Reference Point")]
    public Transform cameraPivot;  //this will be where view bob is appied (camera pivot)
    public CharacterController playerCharControl;

    private float bobTimer = 0f;
    private Vector3 bobOffset = Vector3.zero;
    private Vector3 currentBobOffset = Vector3.zero;
    private Vector3 pivotRestPosition;


    private void Start()
    {
        if (cameraPivot != null)
        {
            pivotRestPosition = cameraPivot.localPosition;
        }
    }

    private void Update()
    {
        HandleBob();
    }

    void HandleBob()
    {
        if (cameraPivot == null || playerCharControl == null) return;

        Vector3 horizontalVelocity = new Vector3(playerCharControl.velocity.x,0f,playerCharControl.velocity.z);
        bool isMoving = horizontalVelocity.magnitude > 0.1f && playerCharControl.isGrounded;

        if (isMoving)
        {
            float speedFactor = Mathf.Clamp01(horizontalVelocity.magnitude / 5f);
            bobTimer += Time.deltaTime * bobFrequency * speedFactor * Mathf.PI * 2f;

            float verticalBob = Mathf.Sin(bobTimer) * bobVerticalAmplitude;
            float horizontalBob = Mathf.Sin(bobTimer * 0.5f) * bobHorizontalAmplitude;

            bobOffset = new Vector3(horizontalBob, verticalBob, 0f);
        }
        else
        {
            bobTimer = Mathf.Lerp(bobTimer, 0f, Time.deltaTime * returnSmoothing);
            bobOffset = Vector3.zero;
        }

        currentBobOffset = Vector3.Lerp(currentBobOffset, bobOffset, Time.deltaTime * bobSmoothing);

        cameraPivot.localPosition = pivotRestPosition + currentBobOffset;
    }
}
