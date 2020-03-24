using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControl : MonoBehaviour
{

    public GameObject thePlayer;
    public float horizontalMove, verticalMove;
    public int stepNum;

    //Flags
    public bool isStepping = false;
    public bool isRunning;

    public AudioSource footStep1;
    public AudioSource footStep2;
    public AudioSource footStep3;
    public AudioSource footStep4;

    private string idleAnimName = "";

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Horizontal") ||
            Input.GetButton("Vertical"))
        {
            thePlayer.GetComponent<Animator>()
                .Play("RunNinja");
            horizontalMove = Input.GetAxis("Horizontal") * Time.deltaTime * 150;
            verticalMove = Input.GetAxis("Vertical") * Time.deltaTime * 8;
            isRunning = true;
            if (!isStepping)
            {
                StartCoroutine(RunSound());
            }

            transform.Rotate(0, horizontalMove, 0);
            transform.Translate(0, 0, verticalMove);

        }
        else
        {
            if (isRunning)
            {
                isRunning = false;

                int val = Random.Range(1, 5);

                switch (val)
                {
                    case 0: idleAnimName = "Victory Idle"; break;
                    case 1: idleAnimName = "Kneeling Idle"; break;
                    case 2: idleAnimName = "Goalkeeper Idle"; break;
                    case 3: idleAnimName = "Ninja Idle"; break;
                    case 4: idleAnimName = "Happy Idle"; break;
                    case 5: idleAnimName = "Sleeping Idle"; break;
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
        yield return new WaitForSeconds(0.3f);
        isStepping = false;
    }
}
