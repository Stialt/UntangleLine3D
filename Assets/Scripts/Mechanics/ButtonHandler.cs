using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    
    public void restartGame()
    {
        Debug.Log("Restart clicked");
        GameAreaPuzzle.setRestart(true);
    }
    
}
