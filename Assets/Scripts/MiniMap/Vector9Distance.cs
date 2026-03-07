using TMPro;
using UnityEngine;

public class Vector9Distance : MonoBehaviour
{
    [SerializeField] TMP_Text distanceText;
    [SerializeField] Transform playerPosition;
    [SerializeField] Transform vector9Pos;
    

    // Update is called once per frame
    void Update()
    {
        float dist = Mathf.Round(Vector3.Distance(vector9Pos.position, playerPosition.position));
        distanceText.text = $"{dist} meters";
    }
}
