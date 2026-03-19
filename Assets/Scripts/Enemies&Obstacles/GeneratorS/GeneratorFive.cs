using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class GeneratorFive : MonoBehaviour
{
    [Header("Generator UI")]
    public GameObject repairAndGenerator;
    public Slider repairPercentage;
    public TMP_Text repairSpeedText;
    public GameObject skillCheck;
    [SerializeField] SkillCheck canRepair;
    public GameObject failedText;
    public GameObject passedText;
    private bool skillCheckRunning;
    private bool waitingForSkillCheck;


    [Header("Base Settings")]
    public GameObject partsNeeded, playerCursor;
    public float repairDuration = 30f;
    public float textDuration = 5f;
    private Material genOneMat;
    [SerializeField] Renderer targetRenderer;
    [SerializeField] CrateFiveUI sparePart;

    [Header("Gen Sounds")]
    public AudioClip genFixing;
    public AudioClip genFixed;
    public AudioSource genFixingSource;

    bool inRange;
    public bool isFifthFixed;
    private bool isPlayingFixingSound;

    [Header("Flickering Lights")]
    public Animator flickeringLights;

    private FPController movement;

    [Header("Gen Upgrade Speed")]
    public FasterGen fastRepairSpeed;

    [Header("Noise Lure")]
    public float hearRadius = 25f;
    public float searchRadius = 6f;

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

        if (genFixingSource == null)
        {
            genFixingSource = gameObject.AddComponent<AudioSource>();
        }

        if (isFifthFixed)
        {
            ApplyFixedState();
        }
        else
        {
            ApplyUnfixedState();
        }
    }

    

    private void Update()
    {
        if (inRange)
        {
            UpdateRepairSpeedtext();
            if (sparePart.partsCollectedFive && !isFifthFixed)
            {
                if (clickAction != null && clickAction.IsPressed())
                {

                    if (!waitingForSkillCheck)
                    {
                        float duration = fastRepairSpeed.GetRepairDuration();
                        float rate = repairPercentage.maxValue / duration;
                        repairPercentage.value += rate * Time.deltaTime;
                    }



                    if (!skillCheckRunning && repairPercentage.value > 0f)
                    {

                        StartCoroutine(SkillCheckRoutine());
                    }



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
                        isFifthFixed = true;

                        ApplyFixedState();

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
            else if (!sparePart.partsCollectedFive)
            {
                if (clickAction != null && clickAction.WasPressedThisFrame())
                {
                    StartCoroutine(ShowPartsMessageFive());
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

    IEnumerator ShowPartsMessageFive()
    {
        repairAndGenerator.SetActive(false);
        partsNeeded.SetActive(true);
        yield return new WaitForSeconds(textDuration);
        partsNeeded.SetActive(false);
    }

    void ApplyFixedState()
    {
        isFifthFixed = true;

        repairAndGenerator.SetActive(false);
        partsNeeded.SetActive(false);

        if (flickeringLights != null)
        {
            flickeringLights.speed = 0;
        }

        if (genOneMat != null)
        {
            genOneMat.EnableKeyword("_EMISSION");
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        if (genFixingSource != null)
        {
            genFixingSource.Stop();
        }


        if (genFixingSource != null && genFixed != null)
        {
            genFixingSource.clip = genFixed;
            genFixingSource.loop = true;
            genFixingSource.spatialBlend = 1f;
            genFixingSource.rolloffMode = AudioRolloffMode.Logarithmic;
            genFixingSource.minDistance = 3f;
            genFixingSource.maxDistance = 20f;
            genFixingSource.dopplerLevel = 0f;
            genFixingSource.Play();
        }

        isPlayingFixingSound = false;
        inRange = false;
    }

    void ApplyUnfixedState()
    {
        if (genOneMat != null)
        {
            genOneMat.DisableKeyword("_EMISSION");
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        if (genFixingSource != null)
        {
            genFixingSource.Stop();
        }

        if (flickeringLights != null)
        {
            flickeringLights.speed = 1;
        }

        isPlayingFixingSound = false;
    }

    public bool GetFixedState()
    {
        return isFifthFixed;
    }

    public void LoadGeneratorState(bool fixedState)
    {
        isFifthFixed = fixedState;

        if (fixedState)
        {
            ApplyFixedState();
        }

        else
        {
            ApplyUnfixedState();
        }
    }

    IEnumerator SkillCheckRoutine()
    {

        skillCheckRunning = true;
        canRepair.hasSkill = false;

        canRepair.failedSkill = false;

        passedText.SetActive(false);
        failedText.SetActive(false);
        skillCheck.SetActive(false);

        yield return new WaitForSeconds(5f);

        waitingForSkillCheck = true;
        skillCheck.SetActive(true);
        yield return new WaitUntil(() => canRepair.hasSkill || canRepair.failedSkill);

        skillCheck.SetActive(false);
        waitingForSkillCheck = false;

        if (canRepair.hasSkill)
        {
            passedText.SetActive(true);
            yield return new WaitForSeconds(4f);
            passedText.SetActive(false);

        }
        else if (canRepair.failedSkill)
        {

            repairPercentage.value = 0;
            failedText.SetActive(true);
            yield return new WaitForSeconds(4f);
            failedText.SetActive(false);


        }

        skillCheckRunning = false;

    }
}
