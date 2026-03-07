using UnityEngine;
using UnityEngine.EventSystems;

public class ViewChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public New3DMap mapCameras;
    public Transform viewPoint;

    public void OnPointerEnter(PointerEventData eventData)
    {
        mapCameras.ChangeView(viewPoint);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mapCameras.ResetView();
    }
}
