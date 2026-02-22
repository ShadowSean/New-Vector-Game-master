using UnityEngine;

public class FireZone : MonoBehaviour
{
    public float zoneLife = 4f;
    public float flameRefreshDuration = 0.6f;
    public float tickRate = 0.15f;

    float despawnTime;
    float nextTick;
    public GameObject particleEffect;

    private void OnEnable()
    {
        despawnTime = Time.time + zoneLife;
    }

    private void Update()
    {
        if (Time.time > despawnTime)
        {
            particleEffect.SetActive(false);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (Time.time < nextTick)
        {
            return;
        }

        Vector9Movement vector9 = other.GetComponentInParent<Vector9Movement>();
        if (vector9 != null)
        {
            vector9.ApplyFireZone(flameRefreshDuration);
            nextTick = Time.time + tickRate;
        }
    }
}
