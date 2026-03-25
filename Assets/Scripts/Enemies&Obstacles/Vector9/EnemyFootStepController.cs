using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class EnemyFootStepController : MonoBehaviour
{
    [Header("Walk and sprint Thresholds")]
    public float walkSpeedMin = 0.1f;
    public float walkSpeedMax = 5.0f;
    public float sprintSpeedMin = 5.2f;

    [Header("Footstep Audio")]
    public AudioSource footstepSource;
    public AudioClip walkClip;
    public AudioClip sprintClip;

    public Transform vector9;
    public Transform u67;

    private WalkCamShake camShake;

    private NavMeshAgent enemyAgent;

    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();

        if (footstepSource == null)
            footstepSource = gameObject.AddComponent<AudioSource>();

        footstepSource.loop = true;
        footstepSource.playOnAwake = false;
    }

    void Update()
    {
        float speed = new Vector3(enemyAgent.velocity.x, 0, enemyAgent.velocity.z).magnitude;

        bool isWalking = speed >= walkSpeedMin && speed < walkSpeedMax;
        bool isSprinting = speed >= sprintSpeedMin;
        float dist = Vector3.Distance(vector9.position, u67.position);

        // Not moving → stop footsteps
        if (speed < walkSpeedMin || enemyAgent.isStopped)
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
