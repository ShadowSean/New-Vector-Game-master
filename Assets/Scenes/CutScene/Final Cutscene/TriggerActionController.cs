using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class MultiTriggerController : MonoBehaviour
{
    [Header("Small Door")]
    public Animator smallDoorAnimator;
    public string smallDoorTriggerTag = "DoorBreak";
    public string smallDoorBreakBool = "break";
    public AudioSource smallDoorAudio;

    [Header("Big Door")]
    public Animator bigDoorAnimator;
    public string bigDoorTriggerTag = "BigDoorOpen";
    public string bigDoorOpenBool = "open";
    public AudioSource bigDoorAudio;
    public AudioSource alarmAudio;
    [Header("Big Door Rumble")]
    public float bigDoorRumbleDuration = 9.3f;
    public float bigDoorRumbleLow = 0.8f;
    public float bigDoorRumbleHigh = 1.0f;
    public float bigDoorThunkLow = 1.0f;
    public float bigDoorThunkHigh = 1.0f;
    public float bigDoorThunkDuration = 0.4f;

    [Header("Cutscene")]
    public string cutsceneTriggerTag = "CutsceneTrigger";
    public PlayableDirector cutsceneTimeline;
    public GameObject canvas;
    public GameObject skipCanvas;
    [Tooltip("This is the already existing cutscene parent object in the scene. It will be turned ON.")]
    public GameObject cutsceneParentToActivate;
    [Tooltip("Optional. If left empty, the script will deactivate this object's parent.")]
    public GameObject playerParentToDeactivate;

    [Header("Options")]
    public bool triggerOnlyOnce = true;

    private bool hasTriggeredCutscene = false;

    private void Start()
    {
        if (cutsceneTimeline != null)
            cutsceneTimeline.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(smallDoorTriggerTag))
        {
            Debug.Log("Small door collision detected");
            if (smallDoorAnimator != null)
            {
                smallDoorAnimator.SetBool(smallDoorBreakBool, true);
                Debug.Log("Set small door bool: " + smallDoorBreakBool);
            }
            else
            {
                Debug.LogWarning("Small Door Animator is not assigned.");
            }

            if (smallDoorAudio != null)
                smallDoorAudio.Play();
            else
                Debug.LogWarning("Small Door AudioSource is not assigned.");

            return;
        }

        if (other.CompareTag(bigDoorTriggerTag))
        {
            Debug.Log("Big door collision detected");
            if (bigDoorAnimator != null)
            {
                bigDoorAnimator.SetBool(bigDoorOpenBool, true);
                Debug.Log("Set big door bool: " + bigDoorOpenBool);
            }
            else
            {
                Debug.LogWarning("Big Door Animator is not assigned.");
            }

            if (bigDoorAudio != null)
                bigDoorAudio.Play();
            
            if (alarmAudio != null)
            {
                alarmAudio.Play();
            }

            else
            {
                Debug.LogWarning("Big Door AudioSource is not assigned.");
            }

            StartCoroutine(BigDoorRumbleSequence());
            return;
        }

        if (other.CompareTag(cutsceneTriggerTag))
        {
            Debug.Log("Cutscene trigger detected");
            if (triggerOnlyOnce && hasTriggeredCutscene) return;

            hasTriggeredCutscene = true;

            if (cutsceneParentToActivate != null)
            {
                cutsceneParentToActivate.SetActive(true);
                skipCanvas.SetActive(true);
                canvas.SetActive(false);
            }

            if (cutsceneTimeline != null)
            {
                cutsceneTimeline.time = 0;
                cutsceneTimeline.Evaluate();
                cutsceneTimeline.Play();
            }

            GameObject targetToDeactivate = playerParentToDeactivate;
            if (targetToDeactivate == null && transform.parent != null)
                targetToDeactivate = transform.parent.gameObject;

            if (targetToDeactivate != null)
                targetToDeactivate.SetActive(false);
        }
    }

    private IEnumerator BigDoorRumbleSequence()
    {
        RumbleManager.Instance.RumbleSequence(bigDoorRumbleLow, bigDoorRumbleHigh, bigDoorRumbleDuration, bigDoorThunkLow, bigDoorThunkHigh, bigDoorThunkDuration); 
        yield return new WaitForSeconds(bigDoorRumbleDuration);
        RumbleManager.Instance.RumblePulse(bigDoorThunkLow, bigDoorThunkHigh, bigDoorThunkDuration);
    }
}