using UnityEngine;
using TMPro;

public class CoinsCollectedText : MonoBehaviour, IDataPersistence
{
    private TextMeshProUGUI coinsCountText;

    public int playerCoins = 0;

    public void LoadData(GameData gameData)
    {
        playerCoins = gameData.playerCoins;
    }

    public void SaveData(GameData gameData)
    {
        gameData.playerCoins = playerCoins;
    }

    private void Start()
    {
        GameObject coinsTextObject = GameObject.Find("Coins Text");

        if (coinsTextObject != null)
        {
            coinsCountText = coinsTextObject.GetComponent<TextMeshProUGUI>();
        }
        UpdateCoinsDisplay();
    }

    private void UpdateCoinsDisplay()
    {
        if (coinsCountText != null)
        {
            coinsCountText.text = "Coins: " + playerCoins;
        }
    }

    public void CollectCoin()
    {
        playerCoins++;
        UpdateCoinsDisplay();
    }
}
