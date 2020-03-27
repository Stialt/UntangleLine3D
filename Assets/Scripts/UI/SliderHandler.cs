using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    public GameObject difficultyValue;

    public void updateSliderValue(float value)
    {
        difficultyValue.GetComponent<Text>().text = "Difficulty: " + (int)value;
        difficultyValue.GetComponent<Text>().fontSize = (int) (24 + (value - 5) / (25) * 8.0);
        GameAreaPuzzle.N = (int)value;
    }
}
