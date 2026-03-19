using UnityEngine;

public class Locker : MonoBehaviour
{
    [SerializeField] Vector9Movement chaseAndDefaultDistance;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chaseAndDefaultDistance.chaseDistance = 0;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Player"))
        //{
        //    chaseAndDefaultDistance = chaseAndDefaultDistance.defaultChaseDistance;
        //}
    }

}
