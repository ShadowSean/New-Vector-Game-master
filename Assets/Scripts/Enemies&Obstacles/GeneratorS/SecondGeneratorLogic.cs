using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.InputSystem;

public class SecondGeneratorLogic : MonoBehaviour
{
    [Header("Generator UI")]
    public GameObject repairAndGenerator;
    public Slider repairPercentage;
    public TMP_Text repairSpeedText;


    [Header("Base Settings")]
    public GameObject partsNeeded, playerCursor;
    public float repairDuration = 30f;
    public float textDuration = 5f;
    private Material genOneMat;
    [SerializeField] Renderer targetRenderer;

    [Header("Gen Sounds")]
    public AudioClip genFixing;
    public AudioClip genFixed;
    public AudioSource genFixingSource;

    bool inRange;
    public bool isSecondFixed;
    private bool isPlayingFixingSound;

    [Header("Flickering Lights")]
    public Animator flickeringLights;

    [Header("Gen Upgrade Speed")]
    public FasterGen fastRepairSpeed;

    private FPController movement;
    private PlayerInput playerInput;
    private InputAction clickAction;

    private void Awake()
    {
        playerInput = FindFirstObjectByType<PlayerInput>();

        if (playerInput != null)
        {
            clickAction = playerInput.actions["Weapon Use"];
        }
    }


    private void Start()
    {
        movement = FindFirstObjectByType<FPController>();

        genOneMat = targetRenderer.material;


        repairAndGenerator.SetActive(false);
        partsNeeded.SetActive(false);
        repairPercentage.gameObject.SetActive(false);

        if (genFixingSource != null)
        {
            genFixingSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnDisable()
    {
        if (genOneMat != null)
        {
            genOneMat.DisableKeyword("_EMISSION");
        }
    }

    private void Update()
    {
        if (inRange)
        {
            UpdateRepairSpeedtext();
            if (CrateTwoUI.partsCollectedTwo && !isSecondFixed)
            {
                if (clickAction != null && clickAction.IsPressed())
                {
                    
                    float duration = fastRepairSpeed.GetRepairDuration();
                    float rate = repairPercentage.maxValue / duration;
                    repairPercentage.value += rate * Time.deltaTime;



                    if (!isPlayingFixingSound && genFixing != null)
                    {
                        genFixingSource.clip = genFixing;
                        genFixingSource.loop = true;
                        genFixingSource.Play();
                        isPlayingFixingSound = true;
                    }

                    if (repairPercentage.value >= repairPercentage.maxValue)
                    {

                        repairPercentage.value = repairPercentage.maxValue;
                        isSecondFixed = true;
                        flickeringLights.speed = 0;

                        repairAndGenerator.SetActive(false);

                        genOneMat.EnableKeyword("_EMISSION");

                        playerCursor.SetActive(true);

                        GetComponent<Collider>().enabled = false;

                        GeneratorCounter.Instance.AddGenerator();


                        if (genFixingSource.isPlaying)
                        {
                            genFixingSource.Stop();
                        }

                        if (genFixed != null)
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
                    if (isPlayingFixingSound)
                    {
                        genFixingSource.Stop();
                        isPlayingFixingSound = false;
                    }
                }
            }
            else if (!CrateTwoUI.partsCollectedTwo)
            {
                if (clickAction != null && clickAction.WasPressedThisFrame())
                {
                    StartCoroutine(ShowPartsMessageTwo());
                }
            }
        }

        else
        {
            if (isPlayingFixingSound)
            {
                genFixingSource.Stop();
                isPlayingFixingSound = false;
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
        }
    }

    void UpdateRepairSpeedtext()
    {
        if (repairSpeedText == null || fastRepairSpeed == null)
        {
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
                isPlayingFixingSound = false;
            }
        }
    }


    IEnumerator ShowPartsMessageTwo()
    {
        repairAndGenerator.SetActive(false);
        partsNeeded.SetActive(true);
        yield return new WaitForSeconds(textDuration);
        partsNeeded.SetActive(false);
    }

    

    
}
