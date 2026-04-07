using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.InputSystem;

public class GeneratorThree : MonoBehaviour
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
    [SerializeField] CrateThreeUI sparePart;

    [Header("Gen Sounds")]
    public AudioClip genFixing;
    public AudioClip genFixed;
    public AudioClip skillCheckSound;
    public AudioSource genFixingSource;

    bool inRange;
    public bool isThirdFixed;
    private bool isPlayingFixingSound;

    [Header("Flickering Lights")]
    public Animator flickeringLights;

    [Header("Gen Upgrade Speed")]
    public FasterGen fastRepairSpeed;

    [Header("Proximity Hum Rumble")]
    [Range(0f, 0.5f)] public float maxHumIntensity = 0.15f;
    public float humMaxDistance = 35f;

    private FPController movement;
    private bool isRepairing;

    private PlayerInput playerInput;
    private InputAction clickAction;

    private void Awake()
    {
        playerInput = FindFirstObjectByType<PlayerInput>();
        if (playerInput != null)
            clickAction = playerInput.actions["Weapon Use"];
    }

    private void Start()
    {
        movement = FindFirstObjectByType<FPController>();
        genOneMat = targetRenderer.material;

        repairAndGenerator.SetActive(false);
        partsNeeded.SetActive(false);
        repairPercentage.gameObject.SetActive(false);

        if (genFixingSource == null)
            genFixingSource = gameObject.AddComponent<AudioSource>();

        if (isThirdFixed) ApplyFixedState();
        else ApplyUnfixedState();
    }

    private void Update()
    {
        if (inRange)
        {
            UpdateRepairSpeedtext();

            if (sparePart.partsCollectedThree && !isThirdFixed)
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
                        StartCoroutine(SkillCheckRoutine());

                    if (!isRepairing)
                    {
                        isRepairing = true;
                        RumbleManager.Instance.RumbleConstant(0.15f, 0.25f);
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
                        isThirdFixed = true;
                        isRepairing = false;

                        RumbleManager.Instance.RumbleFadeOut(0.8f, 0.5f, 1f);

                        ApplyFixedState();
                        playerCursor.SetActive(true);
                        GetComponent<Collider>().enabled = false;
                        GeneratorCounter.Instance.AddGenerator();

                        if (genFixingSource.isPlaying) genFixingSource.Stop();

                        if (genFixed != null)
                        {
                            genFixingSource.clip = genFixed;
                            genFixingSource.loop = true;
                            genFixingSource.spatialBlend = 1f;
                            genFixingSource.rolloffMode = AudioRolloffMode.Linear;
                            genFixingSource.minDistance = 3f;
                            genFixingSource.maxDistance = humMaxDistance;
                            genFixingSource.dopplerLevel = 0f;
                            genFixingSource.Play();
                        }

                        isPlayingFixingSound = false;
                    }
                }
                else
                {
                    if (isRepairing)
                    {
                        isRepairing = false;
                        RumbleManager.Instance.StopRumble();
                    }

                    if (isPlayingFixingSound)
                    {
                        genFixingSource.Stop();
                        isPlayingFixingSound = false;
                    }
                }
            }
            else if (!sparePart.partsCollectedThree)
            {
                if (clickAction != null && clickAction.WasPressedThisFrame())
                    StartCoroutine(ShowPartsMessageThree());
            }

            if (isThirdFixed && movement != null)
            {
                float dist = Vector3.Distance(transform.position, movement.transform.position);
                float t = Mathf.Clamp01(dist / humMaxDistance);
                float intensity = Mathf.Lerp(maxHumIntensity, 0f, t);
                RumbleManager.Instance.RumbleConstant(intensity * 0.5f, intensity);
            }
        }
        else
        {
            if (isRepairing)
            {
                isRepairing = false;
                RumbleManager.Instance.StopRumble();
            }

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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCursor.SetActive(true);
            inRange = false;
            repairAndGenerator.SetActive(false);
            repairPercentage.gameObject.SetActive(false);
            partsNeeded.SetActive(false);

            RumbleManager.Instance.StopRumble();
            isRepairing = false;

            if (isPlayingFixingSound)
            {
                genFixingSource.Stop();
                isPlayingFixingSound = false;
            }
        }
    }

    void UpdateRepairSpeedtext()
    {
        if (repairSpeedText == null || fastRepairSpeed == null) return;

        float duration = fastRepairSpeed.GetRepairDuration();
        int batteries = fastRepairSpeed.batteryCount;
        float speedBoostPercent = Mathf.Max(0f, (repairDuration / duration - 1f) * 100f);
        repairSpeedText.text = $"Upgrade +{speedBoostPercent:0}% speed (Batteries: {batteries})";
    }

    IEnumerator ShowPartsMessageThree()
    {
        repairAndGenerator.SetActive(false);
        partsNeeded.SetActive(true);
        yield return new WaitForSeconds(textDuration);
        partsNeeded.SetActive(false);
    }

    void ApplyFixedState()
    {
        isThirdFixed = true;
        repairAndGenerator.SetActive(false);
        partsNeeded.SetActive(false);

        if (flickeringLights != null) flickeringLights.speed = 0;
        if (genOneMat != null) genOneMat.EnableKeyword("_EMISSION");

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        if (genFixingSource != null) genFixingSource.Stop();

        if (genFixingSource != null && genFixed != null)
        {
            genFixingSource.clip = genFixed;
            genFixingSource.loop = true;
            genFixingSource.spatialBlend = 1f;
            genFixingSource.rolloffMode = AudioRolloffMode.Linear;
            genFixingSource.minDistance = 3f;
            genFixingSource.maxDistance = humMaxDistance;
            genFixingSource.dopplerLevel = 0f;
            genFixingSource.Play();
        }

        isPlayingFixingSound = false;
        inRange = false;
    }

    void ApplyUnfixedState()
    {
        if (genOneMat != null) genOneMat.DisableKeyword("_EMISSION");

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = true;
        if (genFixingSource != null) genFixingSource.Stop();
        if (flickeringLights != null) flickeringLights.speed = 1;

        isPlayingFixingSound = false;
        if (repairPercentage != null)
        {
            repairPercentage.value = 0f;
        }
        skillCheckRunning = false;
        waitingForSkillCheck = false;
    }

    public bool GetFixedState() => isThirdFixed;

    public void LoadGeneratorState(bool fixedState)
    {
        isThirdFixed = fixedState;
        if (fixedState) ApplyFixedState();
        else ApplyUnfixedState();
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

        if (isThirdFixed)
        {
            skillCheck.SetActive(false);
            waitingForSkillCheck = false;
            skillCheckRunning = false;
            yield break;
        }

        waitingForSkillCheck = true;
        skillCheck.SetActive(true);

        if (skillCheckSound != null && genFixingSource != null)
        {
            genFixingSource.PlayOneShot(skillCheckSound);
        }

        yield return new WaitUntil(() => canRepair.hasSkill || canRepair.failedSkill);

        skillCheck.SetActive(false);
        waitingForSkillCheck = false;

        if (canRepair.hasSkill)
        {
            RumbleManager.Instance.RumblePulse(0.1f, 0.7f, 0.15f);
            passedText.SetActive(true);
            yield return new WaitForSeconds(4f);
            passedText.SetActive(false);
            RumbleManager.Instance.RumbleConstant(0.15f, 0.25f);
        }
        else if (canRepair.failedSkill)
        {
            RumbleManager.Instance.RumblePulse(0.9f, 0.4f, 0.3f);
            repairPercentage.value = 0;
            failedText.SetActive(true);
            yield return new WaitForSeconds(4f);
            failedText.SetActive(false);
        }

        skillCheckRunning = false;
    }
}