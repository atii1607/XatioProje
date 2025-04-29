using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelCountText : MonoBehaviour, IDataPersistence
{
    private TextMeshProUGUI livesCountText;
    private TextMeshProUGUI coinsCountText;

    [SerializeField] private int playerLives = 3;
    [SerializeField] private int playerBerry = 0;
    [SerializeField] private int playerCoins = 0;
    [SerializeField] private GameObject restartGame;
    [SerializeField] private PlayerMovement playerMovement;

    private Vector3 respawnPosition = new Vector3(-10.49f, -5.09f, 0f);
    private bool isDead = false;

    public void SaveData(GameData gameData)
    {
        gameData.playerLives = this.playerLives;
        gameData.playerCoins = this.playerCoins;
    }

    public void LoadData(GameData gameData)
    {
        foreach (KeyValuePair<string, bool> pair in gameData.coinsCollected)
        {
            if (pair.Value)
            {
                playerCoins++;
            }
        }

        foreach (KeyValuePair<string, bool> pair in gameData.berryCollected)
        {
            if (pair.Value)
            {
                playerBerry++;
            }
        }

        this.playerLives = gameData.playerLives;
    }

    private void Start()
    {
        if (restartGame != null)
        {
            restartGame.SetActive(false);
        }
        GameObject livesTextObject = GameObject.Find("Lives Text");
        if (livesTextObject != null)
        {
            livesCountText = livesTextObject.GetComponent<TextMeshProUGUI>();
        }

        GameObject coinsTextObject = GameObject.Find("Coins Text");
        if (coinsTextObject != null)
        {
            coinsCountText = coinsTextObject.GetComponent<TextMeshProUGUI>();
        }

        UpdateLivesDisplay();
        UpdateCoinsDisplay();
    }
    public void ProcessPlayerDeath()
    {
        if (isDead)
        {
            return;
        }

        TakeLife();

        if (playerLives > 0)
        {
            StartCoroutine(RespawnPlayerAfterDelay(3f));
        }
        else
        {
            isDead = true;
            restartGame.SetActive(true);
        }
    }


    private IEnumerator RespawnPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerMovement.ResetPlayer(respawnPosition);
    }


    public void OnRestartButtonClicked()
    {
        isDead = false;
        restartGame.SetActive(false);
        RespawnLife();
        playerMovement.ResetPlayer(respawnPosition);

    }
    public void CollectCoin()
    {
        playerCoins++;
        UpdateCoinsDisplay();
    }

    public void TakeLife()
    {
        playerLives--;
        UpdateLivesDisplay();
    }


    public void RespawnLife()
    {
        playerLives = 3;
        UpdateLivesDisplay();
    }

    void UpdateLivesDisplay()
    {
        if (livesCountText != null)
        {
            livesCountText.text = "Lives: " + playerLives;
        }
    }

    void UpdateCoinsDisplay()
    {
        if (coinsCountText != null)
        {
            coinsCountText.text = "Coins: " + playerCoins;
        }
    }
}