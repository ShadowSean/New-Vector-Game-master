using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class LightBehaviour : MonoBehaviour
{
    Light newFlashlight;

    public GameObject batteryUI;

    public bool drainOvertime;
    public float maxBrightness;
    public float minBrightness;
    public float drainRate;
    public GameObject batteryInt;

    public GameObject battery;
    public Slider batteryBar;
    bool canReplaceBattery;
    GameObject nearBattery;

    private void Start()
    {
        
        newFlashlight = GetComponent<Light>();
    }

    private void Update()
    {
        newFlashlight.intensity = Mathf.Clamp(newFlashlight.intensity, minBrightness, maxBrightness);
        if (drainOvertime == true && newFlashlight.enabled == true)
        {
            if (newFlashlight.intensity > minBrightness)
            {
                newFlashlight.intensity -= Time.deltaTime * (drainRate / 1000);
            }
        }

        // Helps batterybar drain properly (fixes direction of drainage issue)
        batteryBar.value = Mathf.InverseLerp(minBrightness,maxBrightness,newFlashlight.intensity);
       
        

        
    }
   

    public void ReplaceBatteryFull()
    {
        newFlashlight.intensity = maxBrightness;
    }
}
