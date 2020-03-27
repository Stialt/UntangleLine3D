using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControl : MonoBehaviour
{

    public GameObject cameraHolder;

    private int NORMAL = 0;
    private int FRONTAL = 1;
    private int ABOVE = 2;

    public int camState = 0;

    void Update()
    {

        //[0.5.0] Disable character control if UI is active
        if (ButtonHandler.victoryActive || ButtonHandler.startActive)
            return;

        //ABOVE Camera
        if (Input.GetMouseButtonDown(0) && (camState == NORMAL || camState == ABOVE))
        {

            // Check if the mouse was clicked over a UI element
            if (!EventSystem.current.IsPointerOverGameObject())
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

            
            
            
        }
        //FRONTAL Camera
        else if (Input.GetMouseButtonDown(1) && (camState == NORMAL || camState == FRONTAL))
        {
            // Check if the mouse was clicked over a UI element
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                cameraHolder.transform.Rotate(cameraHolder.transform.rotation.x, 180, cameraHolder.transform.rotation.z);
                if (camState == NORMAL)
                    camState = FRONTAL;
                else
                    camState = NORMAL;
            }
        }
        //TODO: [0.5.0] Remove useless lines
        /*
        else
        {
            cameraHolder.transform.Rotate(0,0,0);
        }
        */

    }
}
