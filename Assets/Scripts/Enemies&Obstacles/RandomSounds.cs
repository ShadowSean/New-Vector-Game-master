using System.Collections;
using UnityEngine;

public class RandomSounds : MonoBehaviour
{
    public Transform[] audioTransforms;
    public AudioClip[] sounds;

    public float waitTime = 10f;

    private void Start()
    {
        StartCoroutine(PlaySounds());
    }

    IEnumerator PlaySounds()
    {
        yield return null;

        while (true)
        {
            if (audioTransforms == null || audioTransforms.Length == 0)
            {
                yield return new WaitForSeconds(waitTime);
                continue;
            }

            Transform soundTransform = audioTransforms[Random.Range(0,audioTransforms.Length)];
            if (soundTransform == null)
            {
                yield return new WaitForSeconds(waitTime);
                continue;
            }

            AudioSource source = soundTransform.GetComponent<AudioSource>();

            AudioClip randomClipPlay = null;
            if (sounds != null && sounds.Length > 0)
            {
                randomClipPlay = sounds[Random.Range(0 ,sounds.Length)];

            }
            else
            {
                randomClipPlay = source.clip;
            }

            if (randomClipPlay != null)
            {
                source.PlayOneShot(randomClipPlay);
            }

            yield return new WaitForSeconds (waitTime);
        }
    }
}
