using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreGain : MonoBehaviour
{
    private ScoreCountText scoreCountText;

    private void Start()
    {
        scoreCountText = FindObjectOfType<ScoreCountText>();
    }

    private void GainScore()
    {
        scoreCountText.GainScore();
    }
}
