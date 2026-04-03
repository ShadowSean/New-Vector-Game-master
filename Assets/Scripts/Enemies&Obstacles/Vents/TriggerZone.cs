using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerZone : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Animator on the target GameObject to animate.")]
    public Animator animator;

    [Tooltip("AudioSource to play the sound from.")]
    public AudioSource audioSource;


    private void Start()
    {
        Debug.Log(gameObject);
    }


    //public bool hasTriggered = false;

    private void Reset()
    {
        BoxCollider bc = GetComponent<BoxCollider>();
        if (bc != null) bc.isTrigger = true;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Barel trigger entered");
        if (other.CompareTag("Player"))
        {
            PlayAnimation();
            PlaySound();
            Debug.Log("Valid");
        }
       
        //if (hasTriggered) return;
        //if (!other.CompareTag("Player")) return;

        


        //hasTriggered = true;

        
    }

    public void PlayAnimation()
    {
        if (animator == null)
        {
            Debug.LogWarning($"[TriggerZone] No Animator assigned on '{gameObject.name}'.");
            return;
        }
        animator.SetBool("Roll",true);
        //StartCoroutine(ResetRoll());
    }


    //IEnumerator ResetRoll()
    //{
    //    AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
    //    yield return new WaitForSeconds(state.length);
    //    animator.SetBool("Roll",false);
    //}


    private void PlaySound()
    {
        if (audioSource == null)
        {
            Debug.LogWarning($"[TriggerZone] No AudioSource assigned on '{gameObject.name}'.");
            return;
        }

        if (audioSource.clip == null)
        {
            Debug.LogWarning($"[TriggerZone] AudioSource on '{gameObject.name}' has no clip assigned.");
            return;
        }

        audioSource.Play();
    }

    //public void ResetTrigger()
    //{
    //    hasTriggered = false;
    //    animator.SetBool("Roll",false);
    //}
}