using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RestartGame : MonoBehaviour
{
    [SerializeField] private GameObject restartGame;
    [SerializeField] private PlayerMovement playerMovement;
    private LivesCountText livesCountText;

    private Vector3 respawnPosition = new Vector3(-11.5f, -5.5f, 0f);

    private void Start()
    {
        if (restartGame != null)
        {
            restartGame.SetActive(false);
        }
        livesCountText = FindObjectOfType<LivesCountText>();
    }

    public void RestartScreen()
    {
        restartGame.SetActive(true);
    }
    public void OnRestartButtonClicked()
    {
        livesCountText.isDead = false;
        restartGame.SetActive(false);
        livesCountText.RespawnLife();
        playerMovement.ResetPlayer(respawnPosition);
    }

}
