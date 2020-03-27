using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{

    public GameObject startScreen;
    public GameObject victoryScreen;

    public static bool startActive = true;
    public static bool victoryActive = false;

    public void restartGame()
    {
        //Debug.Log("Restart clicked");
        //GameAreaPuzzle.setRestart(true);
        startScreen.SetActive(true);
        victoryScreen.SetActive(false);

        startActive = true;
        victoryActive = false;
    }

    public void startGame()
    {
        GameAreaPuzzle.setRestart(true);
        startScreen.SetActive(false);

        startActive = false;
    }
    
}
