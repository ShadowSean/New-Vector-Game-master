using UnityEngine;

public class FasterGen : MonoBehaviour
{
    public float baseRepairDuration = 30f;
    public float repairBonus = 2f;
    public int batteryCount;

    public float GetRepairDuration()
    {
        return baseRepairDuration - batteryCount * repairBonus;
    }

    public void AddBattery()
    {
        batteryCount++;
    }
}
