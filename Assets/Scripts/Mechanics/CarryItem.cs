using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryItem : MonoBehaviour
{

    public GameObject cube01;
    public GameObject thePlayer;

    public bool isCarrying = false;

    void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {
            if (isCarrying == false)
            {
                if (getDistance(cube01, thePlayer) < 2.0) {
                    Debug.Log("Picked Up");
                    isCarrying = true;
                    //cube01.transform.position.Set(cube01.transform.position.x, 5f, cube01.transform.position.z);
                    cube01.transform.position = new Vector3(cube01.transform.position.x, 2f, cube01.transform.position.z);

                }
            }
            else
            {
                //cube01.transform.parent = null;
                Debug.Log("Dropped");
                cube01.transform.position = new Vector3(thePlayer.transform.position.x, 0.5f, thePlayer.transform.position.z);
                //cube01.transform.position.Set(cube01.transform.position.x, 2f, cube01.transform.position.z);
                isCarrying = false;
            }
        }

        if (isCarrying)
        {

            cube01.transform.position = new Vector3(thePlayer.transform.position.x, 2f, thePlayer.transform.position.z);

        }

    }

    public double getDistance(GameObject A, GameObject B)
    {
        double sum = Mathf.Pow(A.transform.position.x - B.transform.position.x, 2);
        sum += Mathf.Pow(A.transform.position.y - B.transform.position.y, 2);
        sum += Mathf.Pow(A.transform.position.z - B.transform.position.z, 2);

        return Mathf.Sqrt((float) sum);
    }
}
