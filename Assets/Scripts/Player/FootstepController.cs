using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FootstepController : MonoBehaviour
{
    [Header("Walk and Sprint Thresholds")]
    public float walkSpeedMin = 0.1f;
    public float walkSpeedMax = 5.0f;
    public float sprintSpeedMin = 5.2f;

    [Header("Footstep Audio")]
    public AudioSource footstepSource;
    public AudioClip walkClip;
    public AudioClip sprintClip;

    [Header("Footstep Rumble")]
    [Range(0f, 1f)] public float walkRumbleLow = 0.1f;
    [Range(0f, 1f)] public float walkRumbleHigh = 0.2f;
    public float walkRumbleDuration = 0.12f;
    public float walkRumbleInterval = 0.4f;

    [Range(0f, 1f)] public float sprintRumbleLow = 0.3f;
    [Range(0f, 1f)] public float sprintRumbleHigh = 0.4f;
    public float sprintRumbleDuration = 0.1f;
    public float sprintRumbleInterval = 0.28f;

    private CharacterController controller;
    private float footstepRumbleTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (footstepSource == null)
            footstepSource = gameObject.AddComponent<AudioSource>();

        footstepSource.loop = true;
        footstepSource.playOnAwake = false;
    }

    void Update()
    {
        float speed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;

        bool isGrounded = controller.isGrounded;
        bool isWalking = speed >= walkSpeedMin && speed < walkSpeedMax;
        bool isSprinting = speed >= sprintSpeedMin;

        if (speed < walkSpeedMin)
        {
            StopFootsteps();
        }
        else if (isWalking)
        {
            PlayClipIfDifferent(walkClip);
        }
        else if (isSprinting)
        {
            PlayClipIfDifferent(sprintClip);
        }
        else
        {
            StopFootsteps();
        }


        if (isGrounded && (isWalking || isSprinting))
        {
            footstepRumbleTimer -= Time.deltaTime;

            if (footstepRumbleTimer <= 0f)
            {
                if (isSprinting)
                {
                    RumbleManager.Instance.RumblePulse(sprintRumbleLow, sprintRumbleHigh, sprintRumbleDuration);
                    footstepRumbleTimer = sprintRumbleInterval;
                }
                else
                {
                    RumbleManager.Instance.RumblePulse(walkRumbleLow, walkRumbleHigh, walkRumbleDuration);
                    footstepRumbleTimer = walkRumbleInterval;
                }
            }
        }
        else
        {
            footstepRumbleTimer = 0f;
        }
    }

    void PlayClipIfDifferent(AudioClip newClip)
    {
        if (footstepSource.clip == newClip && footstepSource.isPlaying)
            return;

        footstepSource.clip = newClip;
        footstepSource.Play();
    }

    void StopFootsteps()
    {
        if (footstepSource.isPlaying)
            footstepSource.Stop();
    }
}