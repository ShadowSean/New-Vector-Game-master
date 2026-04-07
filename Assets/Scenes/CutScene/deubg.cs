using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deubg : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("TheShipSectionOne");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
