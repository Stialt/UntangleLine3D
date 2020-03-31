using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{

    public GameObject thePlayer;
    private Animator animator;

    public const int IDLE = 0, IDLE_CARRYING = 1, RUNNING = 2,
        RUNNING_FAST = 3, RUNNING_CARRYING = 4, JUMPING_INPLACE = 5, JUMPING_RUNNING = 6;

    public static int PLAYER_STATE = 0;
    public static int DIRECTION_STATE = 0;

    public static int PREVIOUS_STATE = 0;

    void Start()
    {
        animator = thePlayer.GetComponent<Animator>();    
    }

    void Update()
    {
        string animName;

        if (PLAYER_STATE != PREVIOUS_STATE)
        {
            PREVIOUS_STATE = PLAYER_STATE;
            switch (PLAYER_STATE)
            {
                case IDLE:
                    animName = getRandomIdleAnim(1, 5);
                    break;
                case IDLE_CARRYING:
                    animName = "CarryingIdle";
                    break;
                case RUNNING:
                    animName = "RunNinja";
                    break;
                case RUNNING_FAST:
                    animName = "Fast Run";
                    break;
                case RUNNING_CARRYING:
                    animName = "RunWIthObject";
                    break;
                case JUMPING_RUNNING:
                    animName = "JumpNinja";
                    //animName = "JumpWide";
                    break;
                case JUMPING_INPLACE:
                    animName = "JumpInPlace";
                    break;
                default:
                    animName = getRandomIdleAnim(1, 5);
                    break;
            }
            animator.Play(animName);
        }
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
