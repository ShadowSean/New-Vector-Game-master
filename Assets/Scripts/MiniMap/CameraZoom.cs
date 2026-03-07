using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    Camera mapCam;
    
    

    private void Start()
    {
        mapCam = GetComponent<Camera>();
    }



    public void SliderZoom(float t)
    {
       mapCam.fieldOfView = Mathf.Lerp(80f, 30f, t);
    }
}
