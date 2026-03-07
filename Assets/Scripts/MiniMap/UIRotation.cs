using UnityEngine;

public class UIRotation : MonoBehaviour
{
    private bool rotate;
    private float rotateDirection;

    // Update is called once per frame
    void Update()
    {
        if (rotate)
        {
            transform.Rotate(new Vector3(0,rotateDirection * 5f,0));
        }
    }

    public void LeftRotation()
    {
        rotateDirection = -1f;
        rotate = true;
    }
    public void RightRotation()
    {
        rotateDirection = 1f;
        rotate = true;
    }

    public void onRelease()
    {
        rotate = false;
    }
}
