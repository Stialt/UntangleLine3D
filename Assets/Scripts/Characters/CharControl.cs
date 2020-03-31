using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CharControl : MonoBehaviour
{

    public GameObject thePlayer;
    public int stepNum;

    //Flags
    public bool isStepping = false;     //true if step sound is currently playing
    public bool isRunning;
    private bool fastRunning = false;
    private bool isJumping = false;

    public AudioSource footStep1;
    public AudioSource footStep2;
    public AudioSource footStep3;
    public AudioSource footStep4;

    private string idleAnimName = "";

    private Stopwatch stopwatch = new Stopwatch();
    private long normalIdleTimeStamp = 0;

    public float speed = 40.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float camRotateSpeed = 180;
    private float ZMove = 0, XMove = 0, YMove = 0;
    private float distToGround;

    private CharacterController characterController;



    private void Start()
    {
        //[0.6.0] Sleeping State is temporarily removed 
        //stopwatch.Start();

        GetComponent<Rigidbody>().inertiaTensorRotation = Quaternion.identity;
        distToGround = thePlayer.GetComponent<BoxCollider>().bounds.extents.y;

        characterController = thePlayer.GetComponent<CharacterController>();

    }

    void Update()
    {
        //AnimationControl.PLAYER_STATE = AnimationControl.IDLE;


        //[0.5.0] Disable character control if UI is active
        

        if (characterController.isGrounded)
        {
            AnimationControl.PLAYER_STATE = AnimationControl.IDLE;
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {

                if (ButtonHandler.victoryActive || ButtonHandler.startActive)
                    return;

                isRunning = true;

                //Animation State Control
                if (CarryItem.isCarrying)
                {
                    AnimationControl.PLAYER_STATE = AnimationControl.RUNNING_CARRYING;
                    fastRunning = false;
                }
                else if (Input.GetButton("Shift"))
                {
                    AnimationControl.PLAYER_STATE = AnimationControl.RUNNING_FAST;
                    fastRunning = true;
                }
                else
                {
                    AnimationControl.PLAYER_STATE = AnimationControl.RUNNING;
                    fastRunning = false;
                }
                //~~~ Animation State Control

                //[0.6.0] Character rotation now performed with mouse
                //horizontalMove = Input.GetAxis("Horizontal") * Time.deltaTime * 150;


                ZMove = Input.GetAxis("Vertical") * Time.deltaTime * 8;
                XMove = Input.GetAxis("Horizontal") * Time.deltaTime * 8;
            }
            else
            {
                //No movement with W,A,S,D
                fastRunning = false;
                if (isRunning)
                {
                    isRunning = false;
                    //normalIdleTimeStamp = stopwatch.ElapsedMilliseconds;

                    AnimationControl.PLAYER_STATE = AnimationControl.IDLE;

                    if (CarryItem.isCarrying)
                    {
                        AnimationControl.PLAYER_STATE = AnimationControl.IDLE_CARRYING;
                    }
                }

                if (!isRunning && CarryItem.isCarrying)
                {
                    AnimationControl.PLAYER_STATE = AnimationControl.IDLE_CARRYING;
                }

                ZMove = XMove = 0;

                //[0.6.0] Sleeping State is temporarily removed 
                /*
                else if (stopwatch.ElapsedMilliseconds - normalIdleTimeStamp > 5000)
                    idleAnimName = "Sleeping Idle";
                */
            }

            if (fastRunning)
            {
                ZMove *= 1.5f;
                XMove *= 1.5f;
            }

            if (Input.GetButton("Jump") && !CarryItem.isCarrying)
            {
                //Jumping
                YMove = jumpSpeed;
                isJumping = true;
                if (isRunning)
                    AnimationControl.PLAYER_STATE = AnimationControl.JUMPING_RUNNING;
                else
                    AnimationControl.PLAYER_STATE = AnimationControl.JUMPING_INPLACE;
            }

        } //~~~ if isGrounded

        //[0.6.0] Character rotation now performed with mouse
        //Camera rotation with mouse
        if (!ButtonHandler.victoryActive && !ButtonHandler.startActive)
        {
            float mouseX = Input.GetAxis("Mouse X");
            if (mouseX < 0) mouseX = -1;
            else if (mouseX > 0) mouseX = 1;
            transform.Rotate(0, mouseX * camRotateSpeed * Time.deltaTime, 0);
        }

        //isRunning = true;
        if (!isStepping && (ZMove != 0 || XMove != 0) && !isJumping)
            StartCoroutine(RunSound());

        YMove -= gravity * Time.deltaTime;
        /*if (IsGrounded())
            YMove = 0.0f;*/

        //[0.6.0] Character rotation now performed with mouse
        //transform.Rotate(0, horizontalMove, 0);

        //[0.6.0] Attempt to use CharacterController
        Vector3 moveDirection = new Vector3(XMove, YMove, ZMove);
        moveDirection = moveDirection * speed;
        moveDirection.y = YMove;
        moveDirection = 
            Quaternion.Euler(
                transform.eulerAngles.x, 
                transform.eulerAngles.y, 
            transform.eulerAngles.z) *
            moveDirection;
        characterController.Move(moveDirection * Time.deltaTime);
        //transform.Translate(XMove, YMove, ZMove); //--
    }


    bool IsGrounded()
    {
        return Physics.Raycast(thePlayer.transform.position, -Vector3.up, distToGround + 0.1f);
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
            yield return new WaitForSeconds(0.21f);
        else
            yield return new WaitForSeconds(0.315f);
        isStepping = false;
    }


}
