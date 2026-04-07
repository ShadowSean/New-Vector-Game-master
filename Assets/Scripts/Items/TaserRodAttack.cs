using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

public class TaserRodAttack : MonoBehaviour
{
    public float stunRange = 3f;
    public float cooldown = 1.5f;      // seconds between stuns
    public LayerMask enemyLayer;
    public Animator taserStunAnim;

    private bool canStun = true;
    private Camera playerCam;

    [Header("Audio Settings")]
    public AudioSource tasersound;
    public AudioClip taserclip;

    private PlayerInput playerInput;
    private InputAction clickAction;
    private bool PlayingAnim;
    [SerializeField] ItemSwitcher swap;

    private void Awake()
    {
        playerInput = FindFirstObjectByType<PlayerInput>();

        if (playerInput != null)
        {
            clickAction = playerInput.actions["Weapon Use"];
        }
        PlayingAnim = false;
    }



    void Start()
    {
        playerCam = Camera.main;
        tasersound = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Only stun when pressing LMB AND cooldown ready
        if (clickAction != null && clickAction.WasPressedThisFrame() && canStun)
        {
            TryStunEnemy();
           
        }
    }



    void TryStunEnemy()
    {
        float capsuleRadius = 0.5f;
        float capsuleHeight = 1.0f;

        Vector3 start = playerCam.transform.position - playerCam.transform.up * 0.5f;
        Vector3 end = playerCam.transform.position + playerCam.transform.up * 0.5f;
        taserStunAnim.SetBool("New Tase", true);
        
       
        StartCoroutine(taserstopstun());
        
        if (Physics.CapsuleCast(start, end, capsuleRadius, playerCam.transform.forward, out RaycastHit hit, stunRange,enemyLayer))
        {
            Vector9Movement enemy = hit.collider.GetComponent<Vector9Movement>();

            if (enemy != null && enemy.isStunned == false)
            {
                
         
                enemy.Stun();
                StartCoroutine(CooldownRoutine());
            }
        }
    }

    public void PlaySound()
    {
        if (PlayingAnim)
        {
            return;
        }
        tasersound.PlayOneShot(taserclip);
        PlayingAnim = true;
    }

    public void StopSound()
    {
        PlayingAnim = false;
    }

    public void Swao()
    {
        swap.Swap(true);
    }

    public void SwaoFalse()
    {
        swap.Swap(false);
    }

    IEnumerator taserstopstun()
    {
        yield return new WaitForSeconds(taserclip.length + 0.13f);
        taserStunAnim.SetBool("New Tase", false);
        


    }
    IEnumerator CooldownRoutine()
    {
        
        canStun = false;
        
        yield return new WaitForSeconds(cooldown);
        canStun = true;
        
    }
}
