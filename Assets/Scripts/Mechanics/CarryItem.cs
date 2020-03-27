using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UntangleLines;

public class CarryItem : MonoBehaviour
{

    GameObject carryItem;
    public GameObject thePlayer;
    public GameObject carryInHands;

    private GameObject[] cubes;

    public static bool isCarrying = false;

    private double PICK_UP_DISTANCE = 1.0;
    private double DROP_DISTANCE = 0.9;
    private float PICK_UP_ANGLE = 45.0f;    //Maximum angle between player direction and object

    private float itemPickUpYAngle;
    private float playerPickUpYAngle;
    private float heightDifferencePickup;

    public static int carryID = -1;

    void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {
            if (isCarrying == false)
            {
                //Find closest item
                cubes = GameObject.FindGameObjectsWithTag("CarryItem");
                carryItem = getClosest(cubes, thePlayer);

                //[0.5.0] Check if the item is in front of the player
                Vector3 directionToTarget = thePlayer.transform.position - carryItem.transform.position;
                float angle = Vector3.Angle(-thePlayer.transform.forward, directionToTarget);
                //if (Mathf.Abs(angle) < 90)
                //    Debug.Log("target is in front of me");

                //Check if in grab range
                if (getDistance(carryItem, thePlayer) < PICK_UP_DISTANCE && Mathf.Abs(angle) < PICK_UP_ANGLE) {
                    //--------PICKED UP---------

                    isCarrying = true;

                    //Get carried carryID if exists
                    if (carryItem.GetComponent<MultipleTags>() != null)
                    {
                        string tag = carryItem.GetComponent<MultipleTags>().GetAtIndex(0);
                        carryID = Int32.Parse(tag);
                    }


                    if (carryItem.GetComponent<MeshCollider>() != null)
                        carryItem.GetComponent<MeshCollider>().enabled = false;
                    if (carryItem.GetComponent<BoxCollider>() != null)
                        carryItem.GetComponent<BoxCollider>().enabled = false;
                    if (carryItem.GetComponentInChildren<BoxCollider>() != null)
                        carryItem.GetComponentInChildren<BoxCollider>().enabled = false;

                    itemPickUpYAngle = carryItem.transform.eulerAngles.y;
                    playerPickUpYAngle = thePlayer.transform.eulerAngles.y;
                    heightDifferencePickup = carryItem.transform.localPosition.y - thePlayer.transform.position.y;
                }
            }
            else
            {
                //-------DROPPED--------
                carryID = -1;

                //Get Player Y Angle
                float playerAngle = thePlayer.transform.eulerAngles.y;


                //Calculate new coordinates of item on drop
                float dropX = (float) (thePlayer.transform.position.x + DROP_DISTANCE * Mathf.Sin((float)((playerAngle) / 180.0 * Mathf.PI)));
                float dropZ = (float) (thePlayer.transform.position.z + DROP_DISTANCE * Mathf.Cos((float)((playerAngle) / 180.0 * Mathf.PI)));


                //Set new coordinates of item
                carryItem.transform.position = new Vector3(dropX, thePlayer.transform.position.y + heightDifferencePickup, dropZ);
                carryItem.transform.rotation = Quaternion.Euler(carryItem.transform.eulerAngles.x, playerAngle - playerPickUpYAngle + itemPickUpYAngle, carryItem.transform.eulerAngles.z);

                //Set new coordinates for point in puzzle
                if (carryItem.GetComponent<MultipleTags>() != null)
                {
                    string tag = carryItem.GetComponent<MultipleTags>().GetAtIndex(0);
                    GameAreaPuzzle.updatePoint(tag, dropX, dropZ);
                }
                
                if (carryItem.GetComponent<MeshCollider>() != null)
                    carryItem.GetComponent<MeshCollider>().enabled = true;
                if (carryItem.GetComponent<BoxCollider>() != null)
                    carryItem.GetComponent <BoxCollider>().enabled = true;
                if (carryItem.GetComponentInChildren<BoxCollider>() != null)
                    carryItem.GetComponentInChildren<BoxCollider>().enabled = true;

                isCarrying = false;
                CharControl.changeToNormalAnimation = true;

                //Victory Condition Check
                GameAreaPuzzle.checkVictory();
            }
        }

        if (isCarrying)
        {
            float playerAngle = thePlayer.transform.eulerAngles.y;
            float dropX = (float)(thePlayer.transform.position.x + 0.3f * Mathf.Sin((float)((playerAngle) / 180.0 * Mathf.PI)));
            float dropZ = (float)(thePlayer.transform.position.z + 0.3f * Mathf.Cos((float)((playerAngle) / 180.0 * Mathf.PI)));

            carryItem.transform.position = 
                new Vector3(dropX, heightDifferencePickup + thePlayer.transform.position.y + 0.8f, dropZ);
            carryItem.transform.rotation = Quaternion.Euler(carryItem.transform.eulerAngles.x, playerAngle - playerPickUpYAngle + itemPickUpYAngle, carryItem.transform.eulerAngles.z);

            //Updating point coordinates for puzzle as well
            if (carryItem.GetComponent<MultipleTags>() != null)
            {
                string tag = carryItem.GetComponent<MultipleTags>().GetAtIndex(0);
                GameAreaPuzzle.updatePoint(tag, dropX, dropZ);
            }
        }

    }

    /// <summary>
    /// Finds closest object from the given array to the given one
    /// </summary>
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

    /// <summary>
    /// Function to calculate geometric distance between two GameObject objects
    /// </summary>
    public double getDistance(GameObject A, GameObject B)
    {
        double sum = Mathf.Pow(A.transform.position.x - B.transform.position.x, 2);
        sum += Mathf.Pow(A.transform.position.y - B.transform.position.y, 2);
        sum += Mathf.Pow(A.transform.position.z - B.transform.position.z, 2);

        return Mathf.Sqrt((float) sum);
    }
}
