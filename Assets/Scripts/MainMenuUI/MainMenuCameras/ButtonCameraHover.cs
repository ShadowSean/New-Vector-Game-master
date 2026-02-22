using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonCameraHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MenuController cameraController;
    public Transform viewPoint;


    public void OnPointerEnter(PointerEventData eventData)
    {
        cameraController.ChangeView(viewPoint);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cameraController.ResetView();
    }

}
