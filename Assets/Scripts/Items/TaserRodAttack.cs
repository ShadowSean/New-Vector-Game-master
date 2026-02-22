using UnityEngine;
using System.Collections;

public class TaserRodAttack : MonoBehaviour
{
    public float stunRange = 3f;
    public float cooldown = 1.5f;      // seconds between stuns
    public LayerMask enemyLayer;

    private bool canStun = true;
    private Camera playerCam;

    [Header("Audio Settings")]
    public AudioSource tasersound;
    public AudioClip taserclip;

    

    void Start()
    {
        playerCam = Camera.main;
        tasersound = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Only stun when pressing LMB AND cooldown ready
        if (Input.GetMouseButtonDown(0) && canStun)
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
        if (Physics.CapsuleCast(start, end, capsuleRadius, playerCam.transform.forward, out RaycastHit hit, stunRange,enemyLayer))
        {
            Vector9Movement enemy = hit.collider.GetComponent<Vector9Movement>();

            if (enemy != null && enemy.isStunned == false)
            {
                tasersound.PlayOneShot(taserclip);
                enemy.Stun();
                StartCoroutine(CooldownRoutine());
            }
        }
    }

    IEnumerator CooldownRoutine()
    {
        canStun = false;
        yield return new WaitForSeconds(cooldown);
        canStun = true;
    }
}
