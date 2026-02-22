using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FootstepController : MonoBehaviour
{
    [Header("Walk and sprint Thresholds")]
    public float walkSpeedMin = 0.1f;
    public float walkSpeedMax = 5.0f;
    public float sprintSpeedMin = 5.2f;

    [Header("Footstep Audio")]
    public AudioSource footstepSource;
    public AudioClip walkClip;
    public AudioClip sprintClip;

    private CharacterController controller;

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
        
        bool isWalking = speed >= walkSpeedMin && speed < walkSpeedMax;
        bool isSprinting = speed >= sprintSpeedMin;

        // Not moving → stop footsteps
        if (speed < walkSpeedMin)
        {
            StopFootsteps();
            return;
        }

        if (isWalking)
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
    }

    void PlayClipIfDifferent(AudioClip newClip)
    {
        if (footstepSource.clip == newClip && footstepSource.isPlaying)
            return;

        // switch audioclip in audiosource
        footstepSource.clip = newClip;
        footstepSource.Play();
    }

    void StopFootsteps()
    {
        if (footstepSource.isPlaying)
            footstepSource.Stop();
    }
}
