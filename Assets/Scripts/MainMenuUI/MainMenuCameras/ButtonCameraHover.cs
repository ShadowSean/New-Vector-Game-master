using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonCameraHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,ISelectHandler,IDeselectHandler
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

    public void OnSelect(BaseEventData eventData)
    {
        cameraController.ChangeView(viewPoint);
    }
 
    public void OnDeselect(BaseEventData eventData)
    {
        cameraController.ResetView();
    }
}
