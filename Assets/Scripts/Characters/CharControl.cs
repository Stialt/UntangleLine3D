using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CharControl : MonoBehaviour
{

    public GameObject thePlayer;
    public float horizontalMove, verticalMove;
    public int stepNum;

    //Flags
    public bool isStepping = false;
    public bool isRunning;
    private bool fastRunning = false;

    public static bool changeToNormalAnimation = false;

    public AudioSource footStep1;
    public AudioSource footStep2;
    public AudioSource footStep3;
    public AudioSource footStep4;

    private string idleAnimName = "";

    private Stopwatch stopwatch = new Stopwatch();
    private long normalIdleTimeStamp = 0;


    private void Start()
    {
        stopwatch.Start();
        GetComponent<Rigidbody>().inertiaTensorRotation = Quaternion.identity;
        idleAnimName = getRandomIdleAnim(1, 5);
    }

    void Update()
    {
        if (Input.GetButton("Horizontal") ||
            Input.GetButton("Vertical"))
        {

            if (CarryItem.isCarrying)
            {
                thePlayer.GetComponent<Animator>().Play("RunWIthObject");
                fastRunning = false;
            }
            else if (Input.GetButton("Shift"))
            {
                thePlayer.GetComponent<Animator>().Play("Fast Run");
                fastRunning = true;
            }
            else
            {
                thePlayer.GetComponent<Animator>().Play("RunNinja");
                fastRunning = false;
            }


            horizontalMove = Input.GetAxis("Horizontal") * Time.deltaTime * 150;
            verticalMove = Input.GetAxis("Vertical") * Time.deltaTime * 8;

            if (fastRunning)
                verticalMove *= 1.5f;

            isRunning = true;
            if (!isStepping)
            {
                StartCoroutine(RunSound());
            }

            transform.Rotate(0, horizontalMove, 0);
            transform.Translate(0, 0, verticalMove);

            //Debug.Log("Player angle is " + thePlayer.transform.eulerAngles.x +
            //    " " + thePlayer.transform.eulerAngles.y + " " + thePlayer.transform.eulerAngles.z);
            //UnityEngine.Debug.Log("Player Y Angle is " + thePlayer.transform.eulerAngles.y);
        }
        else
        {
            fastRunning = false;
            if (isRunning)
            {
                isRunning = false;
                normalIdleTimeStamp = stopwatch.ElapsedMilliseconds;
                idleAnimName = getRandomIdleAnim(1, 5);

                if (CarryItem.isCarrying)
                    idleAnimName = "CarryingIdle";
            }

            if (!isRunning && CarryItem.isCarrying)
            {
                idleAnimName = "CarryingIdle";
            }
            else if (changeToNormalAnimation)
            {
                normalIdleTimeStamp = stopwatch.ElapsedMilliseconds;
                changeToNormalAnimation = false;
                idleAnimName = getRandomIdleAnim(1, 5);
            }
            else
            {
                if (stopwatch.ElapsedMilliseconds - normalIdleTimeStamp > 5000)
                {
                    idleAnimName = "Sleeping Idle";
                }
            }
          

            thePlayer.GetComponent<Animator>().Play(idleAnimName);

        }
    }



    IEnumerator RunSound()
    {
        if (isRunning && !isStepping)
        {
            isStepping = true;
            stepNum = Random.Range(1, 5);


            switch (stepNum)
            {
                case 1: footStep1.Play(); break;
                case 2: footStep2.Play(); break;
                case 3: footStep3.Play(); break;
                case 4: footStep4.Play(); break;
                default: break;
            }
        }
        if (fastRunning)
            yield return new WaitForSeconds(0.2f);
        else
            yield return new WaitForSeconds(0.3f);
        isStepping = false;
    }

    string getRandomIdleAnim(int min, int max)
    {
        int val = Random.Range(min, max);

        string str;

        switch (val)
        {
            case 0: str = "Victory Idle"; break;
            case 1: str = "Kneeling Idle"; break;
            case 2: str = "Goalkeeper Idle"; break;
            case 3: str = "Ninja Idle"; break;
            case 4: str = "Happy Idle"; break;
            case 5: str = "Sleeping Idle"; break;
            default: str = "Ninja Idle"; break;
        }

        return str;
    }
}
