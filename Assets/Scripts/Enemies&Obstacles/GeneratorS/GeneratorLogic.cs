using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeneratorLogic : MonoBehaviour
{
    [Header("Generator UI")]
    public GameObject repairAndGenerator;
    public Slider repairPercentage;
    public TMP_Text repairSpeedText;
    

    [Header("Base Settings")]
    public GameObject partsNeeded, playerCursor;
    public float repairDuration = 30f;
    public float textDuration = 5f;

    [Header("Gen Sounds")]
    public AudioClip genFixing;
    public AudioClip genFixed;
    public AudioSource genFixingSource;

    bool inRange;
    public bool isFixed;
    private bool isPlayingFixingSound;

    [Header("Flickering Lights")]
    public Animator flickeringLight;

    [Header("Gen Upgrade Speed")]
    public FasterGen fastRepairSpeed;

    private FPController movement;
    

    private void Start()
    {
        movement = FindFirstObjectByType<FPController>();
        

        repairAndGenerator.SetActive(false);
        partsNeeded.SetActive(false);
        repairPercentage.gameObject.SetActive(false);

        if(genFixingSource != null)
        {
            genFixingSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (inRange)
        {
            UpdateRepairSpeedtext();
            if (CrateUI.partsCollected && !isFixed)
            {
                if (Input.GetMouseButton(0))
                {
                    if (movement != null)
                    {
                        movement.canMove = false;
                    }
                    float duration = fastRepairSpeed.GetRepairDuration();
                    float rate = repairPercentage.maxValue / duration;
                    repairPercentage.value += rate * Time.deltaTime;



                    

                    if(!isPlayingFixingSound && genFixing != null)
                    {
                        genFixingSource.clip = genFixing;
                        genFixingSource.loop = true;
                        genFixingSource.Play();
                        isPlayingFixingSound = true;
                    }

                    if (repairPercentage.value >= repairPercentage.maxValue)
                    {

                        repairPercentage.value = repairPercentage.maxValue;
                        isFixed = true;
                        flickeringLight.speed = 0;

                        repairAndGenerator.SetActive(false);
                        if (movement != null)
                        {
                            movement.canMove = true;
                        }

                        playerCursor.SetActive(true);

                        GetComponent<Collider>().enabled = false;

                        GeneratorCounter.Instance.AddGenerator();


                        if (genFixingSource.isPlaying)
                        {
                            genFixingSource.Stop();
                        }

                        if(genFixed != null)
                        {
                            genFixingSource.clip = genFixed;
                            genFixingSource.loop = true;
                            genFixingSource.spatialBlend = 1f;
                            genFixingSource.rolloffMode = AudioRolloffMode.Logarithmic;
                            genFixingSource.minDistance = 3f;
                            genFixingSource.maxDistance = 25f;
                            genFixingSource.dopplerLevel = 0f;
                            genFixingSource.Play();
                        }

                        isPlayingFixingSound = false;
                        
                    }
                }

                else
                {
                    if(isPlayingFixingSound)
                    {
                        genFixingSource.Stop();
                        isPlayingFixingSound = false;
                    }
                }
            }
            else if (!CrateUI.partsCollected)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(ShowPartsMessage());
                }
            }
        }
        else
        {
            if(isPlayingFixingSound)
            {
                genFixingSource.Stop();
                isPlayingFixingSound= false;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
         
            playerCursor.SetActive(false);
            inRange = true;
            repairAndGenerator.SetActive(true);
            repairPercentage.gameObject.SetActive(true);

            UpdateRepairSpeedtext();
        }
    }

    void UpdateRepairSpeedtext()
    {
        if (repairSpeedText == null)
        {
            Debug.LogWarning($"{name}: repairSpeedText NOT assigned");
            return;
        }

        if (fastRepairSpeed == null)
        {
            repairSpeedText.text = "No FasterGen assigned!";
            Debug.LogWarning($"{name}: fastRepairSpeed NOT assigned");
            return;
        }

        
        

        float duration = fastRepairSpeed.GetRepairDuration();
        int batteries = fastRepairSpeed.batteryCount;
        float speedBoostPercent = (repairDuration / duration - 1f) * 100f;
        if (speedBoostPercent < 0f)
        {
            speedBoostPercent = 0f;
        }
        repairSpeedText.text = $"Upgrade +{speedBoostPercent:0}% speed (Batteries: {batteries})";

        Debug.Log($"{name}: UI updated, batteries = {batteries}, duration = {duration}");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          
            playerCursor.SetActive(true);
            inRange = false;
            repairAndGenerator.SetActive(false);
            repairPercentage.gameObject.SetActive(false);
            partsNeeded.SetActive(false);

            if (isPlayingFixingSound)
            {
                genFixingSource.Stop();
                isPlayingFixingSound= false;
            }
        }
    }

    IEnumerator ShowPartsMessage()
    {
        repairAndGenerator.SetActive(false);
        partsNeeded.SetActive(true);
        yield return new WaitForSeconds(textDuration);
        partsNeeded.SetActive(false);
    }

    

    

}
