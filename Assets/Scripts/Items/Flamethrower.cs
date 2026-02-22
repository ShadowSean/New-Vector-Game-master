using UnityEngine;
using System.Collections;

public class Flamethrower : MonoBehaviour
{
    [Header("Usage")]
    public float cooldown = 10f;    // cooldown for the flamethrower(will be changed to uses later)
    private bool canSlow = true; // boolean for knowing if slow can be applied

    [Header("Fire Effect")]
    public float fearDuration = 5f;
    public float castRange = 15f;
    public LayerMask enemyLayer;

    [Header("Capsule Raycast Settings")]
    public float capsuleRadius = 0.6f;
    public float capsuleHalfHeight = 0.6f;

    [Header("Particles")]
    public ParticleSystem muzzleParticles;
    public ParticleSystem zoneParticles;

    

    Camera playerCam;

    private void Start()
    {
        playerCam = Camera.main;
    }

    private void OnEnable()
    {
        canSlow = true;

        

        StopAndClear(muzzleParticles);
        StopAndClear(zoneParticles);
    }

    private void OnDisable()
    {
        

        StopAndClear(muzzleParticles);
        StopAndClear(zoneParticles);
    }


    void Update()
    {
        // Only slow when pressing LMB AND cooldown ready
        if (Input.GetMouseButtonDown(0) && canSlow)
        {
            ActivateFlamethrower();
        }
    }

    void ActivateFlamethrower()
    {
        canSlow = false;

        

        //PLay the muzzle particles
        if (muzzleParticles != null)
        {
            muzzleParticles.Play();
        }

        if (zoneParticles != null)
        {
            zoneParticles.Play();
        }

        TryFearVector9();

        StopAllCoroutines();
        StartCoroutine(FlameDurationRoutine());
        StartCoroutine(CooldownRoutine());
    }

    void TryFearVector9()
    {
        if (playerCam == null)
        {
            return;
        }

        Vector3 origin = playerCam.transform.position;
        Vector3 dir = playerCam.transform.forward;

        Vector3 start = origin + (-playerCam.transform.up * capsuleHalfHeight);
        Vector3 end = origin + (playerCam.transform.up * capsuleHalfHeight);

        if (Physics.CapsuleCast(start, end, capsuleRadius, dir, out RaycastHit hit, castRange, enemyLayer))
        {
            Vector9Movement vector9 = hit.collider.GetComponentInParent<Vector9Movement>();
            if (vector9 != null)
            {
                vector9.ApplyFireZone(fearDuration);
            }
        }
    }

    IEnumerator FlameDurationRoutine()
    {
        yield return new WaitForSeconds(5f);
        

        if (muzzleParticles != null)
        {
            muzzleParticles.Stop();
        }

        if (zoneParticles != null)
        {
            zoneParticles.Stop();
        }
    }


    IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldown);
        canSlow = true;
    }

    void StopAndClear(ParticleSystem particleSystem)
    {
        if (particleSystem == null)
        {
            return;
        }
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        particleSystem.Clear(true);
    }
}
