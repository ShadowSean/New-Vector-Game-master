using UnityEngine;

public class New3DMap : MonoBehaviour
{
    [Header("Map Views")]
    public Transform defaultView;
    public Transform sideView;
    public Transform topView;
    public float changeViewSpeed = 2f;

    private Transform targetView;

    private void Start()
    {
        targetView = defaultView;
    }

    void Update()
    {
        // Smooth move and rotate
        transform.position = Vector3.Lerp(transform.position, targetView.position, Time.deltaTime * changeViewSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetView.rotation, Time.deltaTime * changeViewSpeed);
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
