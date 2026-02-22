using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Animator))]
public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeAmount = 0.02f;

    [SerializeField] private float shakeDuration = 3f;

    private const string TutorialKey = "TutorialCompleted";

    public GameObject skipTutorialUI;
    private bool skipped = false;
    private Coroutine tutorialRoutine;

    public GameObject typingText;
    public GameObject sprintUI;
    public GameObject sprintText;
    public GameObject inventoryUI;
    public GameObject inventoryText;
    public GameObject collectionText;

    private Animator anim;

    private Vector3 initalPos;
    bool isShaking;

    private FPController playerMovement;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        initalPos = transform.localPosition;
    }

    private void Start()
    {
        
        playerMovement = FindFirstObjectByType<FPController>();
        anim = gameObject.GetComponent<Animator>();

        if (PlayerPrefs.GetInt(TutorialKey, 0) == 1)
        {
            SkipTutorial();
            return;
        }
        
        
        tutorialRoutine = StartCoroutine(Tutorial());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipTutorial();
        }
        if (isShaking)
        {
            transform.localPosition = initalPos + Random.insideUnitSphere * shakeAmount;
        }
        else
        {
            transform.localPosition = initalPos;
        }
    }

    void SkipTutorial()
    {
        PlayerPrefs.SetInt(TutorialKey, 1);
        PlayerPrefs.Save();
        
        skipped = true;

        if (tutorialRoutine != null)
        {
            StopCoroutine(tutorialRoutine);
        }

        isShaking = false;
        transform.localPosition = initalPos;

        playerMovement.canMove = true;
        anim.enabled = true;

        typingText.SetActive(false);
        sprintUI.SetActive(true);
        sprintText.SetActive(false);
        inventoryUI.SetActive(true);
        inventoryText.SetActive(false);

        skipTutorialUI.SetActive(false);
    }

    IEnumerator Tutorial()
    {
        skipTutorialUI.SetActive(true);

        isShaking = true;
        playerMovement.canMove = false;
        anim.enabled = false;
        
        yield return new WaitForSeconds(shakeDuration);
        isShaking = false;
        
        playerMovement.canMove=true;
        anim.enabled = true;

        typingText.SetActive(true);
        yield return new WaitForSeconds(5f);
        typingText.SetActive(false);

        collectionText.SetActive(true);
        yield return new WaitForSeconds(5f);
        collectionText.SetActive(false);

        playerMovement.canMove = false;
        anim.enabled = false;

        sprintUI.SetActive(true);
        sprintText.SetActive(true);
        yield return new WaitForSeconds(5f);
        
        sprintText.SetActive(false);

        

        inventoryUI.SetActive(true);
        inventoryText.SetActive(true);
        yield return new WaitForSeconds(8f);
        inventoryText.SetActive(false);

        playerMovement.canMove = true;
        anim.enabled = true;
        skipTutorialUI.SetActive(false);
       

    }

   
}
