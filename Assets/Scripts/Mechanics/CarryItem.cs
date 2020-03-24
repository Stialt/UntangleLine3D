using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryItem : MonoBehaviour
{

    GameObject carryItem;
    public GameObject thePlayer;
    public GameObject carryInHands;

    private GameObject[] cubes;

    public static bool isCarrying = false;

    private double PICK_UP_DISTANCE = 1.0;
    private double DROP_DISTANCE = 1.0;

    void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {
            if (isCarrying == false)
            {
                //Find closest item
                if (cubes == null)
                    cubes = GameObject.FindGameObjectsWithTag("CarryItem");
                carryItem = getClosest(cubes, thePlayer);


                //Check if in grab range
                if (getDistance(carryItem, thePlayer) < PICK_UP_DISTANCE) {
                    Debug.Log("Picked Up");
                    isCarrying = true;
                    carryItem.SetActive(false);
                    carryInHands.SetActive(true);
                    carryItem.transform.position = new Vector3(carryItem.transform.position.x, 2f, carryItem.transform.position.z);
                }
            }
            else
            {
                Debug.Log("Dropped");

                //Get Player Y Angle
                float playerAngle = thePlayer.transform.eulerAngles.y;


                //Calculate new coordinates of item on drop
                float dropX = (float) (thePlayer.transform.position.x + DROP_DISTANCE * Mathf.Sin((float)((playerAngle) / 180.0 * Mathf.PI)));
                float dropZ = (float) (thePlayer.transform.position.z + DROP_DISTANCE * Mathf.Cos((float)((playerAngle) / 180.0 * Mathf.PI)));


                //Set new coordinates of item
                carryItem.transform.position = new Vector3(dropX, 0.25f, dropZ);
                carryItem.transform.rotation = new Quaternion(0, 0, 0, 0);

                //Rotate item to face player on drop
                carryItem.transform.Rotate(new Vector3(0, 1, 0), playerAngle);

                carryInHands.SetActive(false);
                carryItem.SetActive(true);
                isCarrying = false;
                CharControl.changeToNormalAnimation = true;
            }
        }

        if (isCarrying)
        {
            carryItem.transform.position = 
                new Vector3(thePlayer.transform.position.x, 2f, thePlayer.transform.position.z);
        }

    }

    public GameObject getClosest(GameObject[] objects, GameObject ThePlayer)
    {
        if (objects == null) return null;

        GameObject closest = objects[0];
        double minDist = getDistance(closest, ThePlayer);

        for (int i = 1; i < objects.Length; i++)
        {
            double temp = getDistance(objects[i], ThePlayer);
            if (temp < minDist)
            {
                minDist = temp;
                closest = objects[i];
            }
        }


        return closest;
    }

    public double getDistance(GameObject A, GameObject B)
    {
        double sum = Mathf.Pow(A.transform.position.x - B.transform.position.x, 2);
        sum += Mathf.Pow(A.transform.position.y - B.transform.position.y, 2);
        sum += Mathf.Pow(A.transform.position.z - B.transform.position.z, 2);

        return Mathf.Sqrt((float) sum);
    }
}
