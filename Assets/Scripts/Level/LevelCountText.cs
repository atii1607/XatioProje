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
    [SerializeField] PlayerMovement playerMovement;

    private Vector3 respawnPosition;

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

        this.playerLives = gameData.playerLives;
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

    public void ProcessPlayerDeath()
    {
        if (playerLives > 0)
        {
            StartCoroutine(RespawnPlayerAfterDelay(3f));
            if(playerLives <= 0)
            {
                StartCoroutine(RespawnAfterGameReset(3f));
            }
        }
    }

    public IEnumerator RespawnPlayerAfterDelay(float delay)
    {
        TakeLife();
        yield return new WaitForSeconds(delay);
        respawnPosition = new Vector3(-10.49f, -5.09f, 0f);
        playerMovement.ResetPlayer(respawnPosition);
    }
    public IEnumerator RespawnAfterGameReset(float delay)
    {
        yield return new WaitForSeconds(delay);
        RespawnLife();
        GameData gameData = new GameData();
        playerMovement.ResetPlayer(gameData.playerPosition);
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
        DataPersistenceManager.instance.SaveGame();
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
