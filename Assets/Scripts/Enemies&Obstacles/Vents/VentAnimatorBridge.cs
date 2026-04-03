using UnityEngine;

public class VentAnimatorBridge : MonoBehaviour
{
    public VentLogic ventLogic;
    public Animator animator;
    public string animationStateName = "YourAnimationStateName";

    private void Awake()
    {
        animator.enabled = false;
    }

    public void SnapToEnd()
    {
        animator.enabled = true;
        animator.Play(animationStateName, 0, 1f);
        animator.Update(0f);
    }

    public void EnableAnimator()
    {
        animator.enabled = true;
    }

    // Animation Event
    public void OnVentAnimationComplete()
    {
        if (ventLogic != null)
            ventLogic.UnlockVent();
    }
}