using UnityEngine;
using UnityEngine.UI;

public class DiffucultyApplier: MonoBehaviour
{
    [Header("Vector9 Settings")]
    [SerializeField] Vector9Movement vectorNineSettings;
    

    [Header("Item Settings")]
    public Transform[] normalPoints;
    public Transform[] hardPoints;
    public Transform[] insanePoints;

    [Header("Patrol Settings")]
    public Transform[] normalPatrol;
    public Transform[] hardPatrol;
    public Transform[] insanePatrol;

    [Header("Item Points")]
    public GameObject[] items;
    public Transform[] itemTransform;

    private void Start()
    {
        switch (DifficultyChanger1.selectDiffculty)
        {
            case Difficulty.Normal:
                ApplyVector9(30, 2f, 10f, 3f,10f,15f, normalPatrol);
                ApplyPoints(normalPoints);
                break;
            case Difficulty.Hard:
                ApplyVector9(35, 0.5f, 50f, 5f,5f,20f, hardPatrol);
                ApplyPoints(hardPoints);
                break;
            case Difficulty.Insane:
                ApplyVector9(50, 0.2f, 200f, 5f, 2f, 100f, insanePatrol);
                ApplyPoints(insanePoints);
                break;
        }
    }

    void ApplyVector9(float chaseDistance, float waitTime, float chaseSpeed, float patrolSpeed, float stunRange, float vectorAccel, Transform[] patrolPoints)
    {
        vectorNineSettings.chaseDistance = chaseDistance;
        vectorNineSettings.waitTime = waitTime;
        vectorNineSettings.vectorChaseSpeed = chaseSpeed;
        vectorNineSettings.vectorPatrolSpeed = patrolSpeed;
        vectorNineSettings.stunRange = stunRange;
        vectorNineSettings.agent.acceleration = vectorAccel;
        vectorNineSettings.patrolAreas = patrolPoints;
    }

    void ApplyPoints(Transform[] points)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (i < points.Length && points[i] != null)
            {
                items[i].SetActive(true);
                items[i].transform.SetPositionAndRotation(points[i].position, points[i].rotation);

            }
            else
            {
                items[i].SetActive(false);
            }
        }
    }
}
