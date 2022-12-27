using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShoot : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ScreenCapture.CaptureScreenshot("Picture.png", 10);
            Debug.Log("ScreenShoot");
        }
    }
}
