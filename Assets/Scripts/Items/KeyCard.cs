using UnityEngine;

public class KeyCard : MonoBehaviour
{
    public GameObject inticon, playerCursor;

    bool isLookedAt;

    public void ShowInteractPromt(bool show)
    {
        if(inticon != null) inticon.SetActive(show);
        if(playerCursor != null) playerCursor.SetActive(!show);
        isLookedAt = show;
    }

    public void Interact()
    {
        Door.keyFound = true;
        if(inticon != null) inticon.SetActive(false);
        if(playerCursor != null) playerCursor.SetActive(true);

        gameObject.SetActive(false);
    }

    
}
