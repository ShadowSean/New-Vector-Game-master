using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("Camera Views")]
    public Transform defaultView;
    public Transform newGameView;
    public Transform moreInfoView;
    public Transform creditsView;

    private Transform targetView;
    public float speed = 3f;

    private void Start()
    {
        targetView = defaultView;
    }

    void Update()
    {
        // Smooth move and rotate
        transform.position = Vector3.Lerp(transform.position, targetView.position, Time.deltaTime * speed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetView.rotation, Time.deltaTime * speed);
    }

    public void ChangeView(Transform newView)
    {
        targetView = newView;
    }

    public void ResetView()
    {
        targetView = defaultView;
    }
}
