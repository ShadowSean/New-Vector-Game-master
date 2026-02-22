using System.Collections;
using UnityEngine;


public class VentLogic : MonoBehaviour
{

    [Header("Player movement")]
    public GameObject player;
    private FPController movement;

    [Header("Teleporter Area")]
    public Transform ventAreaOne;

    [Header("Fade ")]
    public CanvasGroup fadePanel;
    public float fadeDuration = 1f;

    bool isTeleporting;

    private void OnTriggerEnter(Collider other)
    {
        if (isTeleporting)
        {
            return;
        }

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
        isTeleporting = true;
        yield return StartCoroutine(Fade(0, 1));

        TeleportPlayer();

        yield return new WaitForSeconds(0.1f);

        yield return StartCoroutine(Fade(1,0));


        if (movement != null)
        {
            movement.enabled=true;
        }

        isTeleporting =false;
    }

    void TeleportPlayer()
    {
        if (player == null || ventAreaOne == null)
        {
            return ;
        }

        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        player.transform.SetPositionAndRotation(ventAreaOne.position,ventAreaOne.rotation);

        if (controller != null)
        {
            controller.enabled=true;
        }
    }

    IEnumerator Fade(float start, float end)
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(start,end, t / fadeDuration);
            yield return null;
        }
    }
}
