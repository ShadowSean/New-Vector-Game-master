using UnityEngine;

public class GensFixed : MonoBehaviour
{
    [SerializeField] Animator doorAnimation;
    [SerializeField] AudioSource doorSource;
    [SerializeField] AudioClip doorClip;
    public bool opened;

    private void Update()
    {
        if (opened) return;

        if (GeneratorCounter.Instance != null &&
            GeneratorCounter.Instance.FixedCount >= 3)
        {
            opened = true;
            doorSource.PlayOneShot(doorClip);
            doorAnimation.SetBool("isOpen", true);
            
        }
    }
}
