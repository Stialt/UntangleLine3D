using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public GameObject cameraHolder;

    private int NORMAL = 0;
    private int FRONTAL = 1;
    private int ABOVE = 2;

    public int camState = 0;

    void Update()
    {
        //ABOVE Camera
        //Temporarily Commented
        if (Input.GetMouseButtonDown(0) && (camState == NORMAL || camState == ABOVE))
        {
            if (camState == NORMAL)
            {
                cameraHolder.transform.Rotate(60, 0, 0);
                cameraHolder.transform.Translate(0, 5, 0, Space.World);

                camState = ABOVE;
            }
            else if (camState == ABOVE)
            {
                camState = NORMAL;
                cameraHolder.transform.Rotate(300, 0, 0);
                cameraHolder.transform.Translate(0, -5, 0, Space.World);
            }
            
            
        }
        //FRONTAL Camera
        else if (Input.GetMouseButtonDown(1) && (camState == NORMAL || camState == FRONTAL))
        {
            cameraHolder.transform.Rotate(cameraHolder.transform.rotation.x, 180, cameraHolder.transform.rotation.z);
            if (camState == NORMAL)
                camState = FRONTAL;
            else
                camState = NORMAL;
        }
        else
        {
            cameraHolder.transform.Rotate(0,0,0);
        }

    }
}
