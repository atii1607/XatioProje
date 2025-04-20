using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCountText : MonoBehaviour, IDataPersistence
{
    private TextMeshProUGUI livesCountText;
    private TextMeshProUGUI coinsCountText;

    [SerializeField] int playerLives = 3;
    [SerializeField] int playerBerry = 0;
    [SerializeField] int playerCoins = 0;

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

        foreach(KeyValuePair<string, bool> pair in gameData.berryCollected)
        {
            if (pair.Value)
            {
                playerBerry++;
            }
        }
    }

    void Awake()
    {
        livesCountText = this.GetComponent<TextMeshProUGUI>();
        coinsCountText = this.GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
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

    //public void ProcessPlayerDeath(GameObject player)
    //{
    //    if (playerLives > 1)
    //    {
    //        TakeLife();
    //        //RespawnPlayer(player);
    //    }
    //    else
    //    {
    //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //        playerLives = 3;
    //    }
    //}

    public void CollectCoin()
    {
        playerCoins++;
        UpdateCoinsDisplay();
    }

    void TakeLife()
    {
        playerLives--;
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
