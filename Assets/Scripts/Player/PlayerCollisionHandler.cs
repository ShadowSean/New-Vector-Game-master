using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.AI;


public class PlayerCollisionHandler : MonoBehaviour
{
    [Header("Game Over Settings")]
   
    public AudioSource jumpscareSource;   // audio source that will play the jumpscare sound
    public AudioClip jumpscareClip;       // assign your jumpscare audio file here

    public GameObject MainCanvas;

    

    public GameObject cameraObj;



    public GameObject Vector9;

    private bool gameOverTriggered = false;

    private void Start()
    {
        cameraObj.SetActive(false);
        MainCanvas.SetActive(true);
 
    }

    private void OnTriggerEnter(Collider other)
    {
        // This runs whenever the CharacterController bumps into something solid
        if (gameOverTriggered) return;

        if (other.gameObject.CompareTag("Enemy"))
        {
            FinalVector9 stun = other.GetComponent<FinalVector9>();
            if(stun != null && stun.isStunned)
            {
                return;
            }
           
            gameOverTriggered = true;

            if (MainCanvas) MainCanvas.SetActive(false);
            if (jumpscareSource && jumpscareClip)
            {
                jumpscareSource.PlayOneShot(jumpscareClip);
                cameraObj.SetActive(true);

                Animator jumpScare = Vector9.GetComponent<Animator>();
                jumpScare.speed = 1f;
                jumpScare.SetTrigger("Scare");
                //StartCoroutine(FalseCamera());
            }
            Vector9.GetComponent<NavMeshAgent>().isStopped = true;




           

            if(stun != null)
            {
                stun.StartFade();
            }
        }
    }

    //IEnumerator FalseCamera()
    //{
    //    Animator jumpScare = Vector9.GetComponent<Animator>();
    //    yield return new WaitForSeconds(5f);
    //    jumpScare.SetTrigger("Walk");
    //    Vector9.GetComponent<NavMeshAgent>().isStopped = false;
    //    cameraObj.SetActive(false);
    //    MainCanvas.SetActive(true);
    //}


}
