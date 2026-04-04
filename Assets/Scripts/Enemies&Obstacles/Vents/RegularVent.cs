using UnityEngine;
using System.Collections;

public class RegularVent : MonoBehaviour
{
    [Header("Player movement")]
    public GameObject player;
    private FPController movement;

    [Header("Teleporter Area")]
    public Transform ventTeleport;

    [Header("Fade")]
    public CanvasGroup fadePanel;
    public float fadeDuration = 1f;

    

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            movement = other.GetComponent<FPController>();
            if (movement != null)
            {
                movement.enabled = false;
            }
            StartCoroutine(FadeTeleport());
        }
    }

    IEnumerator FadeTeleport()
    {
        
        yield return StartCoroutine(Fade(0, 1));
        TeleportPlayer();
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(Fade(1, 0));
        if (movement != null)
        {
            movement.enabled = true;
        }
        
    }

    void TeleportPlayer()
    {
        if (player == null || ventTeleport == null) return;

        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        player.transform.SetPositionAndRotation(ventTeleport.position, ventTeleport.rotation);

        if (controller != null) controller.enabled = true;
    }

    IEnumerator Fade(float start, float end)
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(start, end, t / fadeDuration);
            yield return null;
        }
    }
}
