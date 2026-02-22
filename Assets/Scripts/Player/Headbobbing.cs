using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Headbobbing : MonoBehaviour
{
    [SerializeField] private Animator cameraAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            StartBobbing();
        }
       else
        {
            StopBobbing();
        }
        
    }

    void StartBobbing()
    {
        cameraAnimator.Play("HeadBobbing");
    }

    void StopBobbing()
    {
        cameraAnimator.Play("New State");
    }
}
