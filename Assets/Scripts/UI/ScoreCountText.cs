using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCountText : MonoBehaviour
{
    private TextMeshProUGUI scoreCountText;

    public int playerScore = 0;

    public void LoadData(GameData gameData)
    {
        this.playerScore = gameData.playerScore;
    }

    public void SaveData(GameData gameData)
    {
        gameData.playerScore = this.playerScore;
    }

    private void Start()
    {
        GameObject scoreTextObject = GameObject.Find("Score Text");

        if (scoreTextObject != null)
        {
            scoreCountText = scoreTextObject.GetComponent<TextMeshProUGUI>();
        }
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreCountText != null)
        {
            scoreCountText.text = "Score: " + playerScore;
        }
    }

    public void GainScore()
    {
        playerScore += 50;
        UpdateScoreDisplay();
    }
}
