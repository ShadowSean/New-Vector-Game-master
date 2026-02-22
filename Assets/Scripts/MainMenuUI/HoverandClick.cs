using UnityEngine;

public class HoverandClick : MonoBehaviour
{
    public AudioSource soundSource;
    public AudioClip hoverSFX;
    public AudioClip clickSFX;

    public void HoverSound()
    {
        soundSource.PlayOneShot(hoverSFX);
    }

    public void ClickSound()
    {
        soundSource.PlayOneShot(clickSFX);
    }
}
