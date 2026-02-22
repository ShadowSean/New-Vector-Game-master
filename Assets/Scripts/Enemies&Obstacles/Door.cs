using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UIElements;

public class Door : MonoBehaviour
{
    //public AudioSource open,close;
    public static bool keyFound;
    public GameObject door_closed, door_opened, intText,playerScope, cardlockedtext;

    //public AudioSource open, close;

    public bool opened, locked;

    private Coroutine autoCloseroutine;
    private void Start()
    {
        keyFound = false;
    }
    
    public void Interact()
    {
        if (opened) return;

        if (locked && !keyFound)
        {
            ShowLockedMessage();
            return;
        }
        OpenDoor();
    }

    void OpenDoor()
    {
        door_closed.SetActive(false);
        door_opened.SetActive(true);
        intText.SetActive(false);
        playerScope.SetActive(true);
        playerScope.SetActive(true);
        opened = true;

        if (autoCloseroutine != null) StopCoroutine(autoCloseroutine);
        autoCloseroutine = StartCoroutine(AuotoClose());
    }

    IEnumerator AuotoClose()
    {
        yield return new WaitForSeconds(4.0f);
        opened = false;
        door_closed.SetActive(true);
        door_opened.SetActive(false);
        //close.Play();
    }

    void ShowLockedMessage()
    {
        cardlockedtext.SetActive(true);
        playerScope.SetActive(false );
        intText.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(HideLockedText() );
    }

    IEnumerator HideLockedText()
    {
        yield return new WaitForSeconds(2f);
        cardlockedtext.SetActive(false);
        playerScope.SetActive(true);
    }

    public void ShowInteractPromt(bool show)
    {
        if(intText != null) intText.SetActive(show);

        if (playerScope != null)
        {
            playerScope.SetActive(!show);
        }
        
        
    }

    private void Update()
    {
        if (keyFound == true)
        {
            locked = false;
        }
    }
}
