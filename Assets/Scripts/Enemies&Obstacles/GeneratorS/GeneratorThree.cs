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
    public bool isThirdFixed;
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

        if (genFixingSource == null)
        {
            genFixingSource = gameObject.AddComponent<AudioSource>();
        }

        if (isThirdFixed)
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
            if (CrateThreeUI.partsCollectedThree && !isThirdFixed)
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
                        isThirdFixed = true;

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
            else if (!CrateThreeUI.partsCollectedThree)
            {
                if (clickAction != null && clickAction.WasPressedThisFrame())
                {
                    StartCoroutine(ShowPartsMessageThree());
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
        return isThirdFixed;
    }

    public void LoadGeneratorState(bool fixedState)
    {
        isThirdFixed = fixedState;

        if (fixedState)
        {
            ApplyFixedState();
        }

        else
        {
            ApplyUnfixedState();
        }
    }
}
