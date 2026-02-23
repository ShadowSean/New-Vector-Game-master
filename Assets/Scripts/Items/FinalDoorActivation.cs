using UnityEngine;

public class FinalDoorActivation : MonoBehaviour
{
    [SerializeField] Animator animator;
    private bool doorDeactivated;

    private void Update()
    {
        if (doorDeactivated) return;

        if (GeneratorCounter.Instance != null &&
            GeneratorCounter.Instance.FixedCount >= GeneratorCounter.Instance.totalGens)
        {
            animator.SetBool("isOpen", true);
            doorDeactivated = true;
        }
    }
}
