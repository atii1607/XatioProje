using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public int playerLives;
    public int playerCoins;
    public string lastSceneName;
    public Vector3 playerPosition;
    public SerializableDictionary<string, bool> coinsCollected;
    public SerializableDictionary<string, bool> berryCollected;

    public GameData()
    {
        this.playerLives = 3;
        this.playerCoins = 0;
        this.lastSceneName = "Level 1";
        playerPosition = new Vector3(-11.5f, -5.5f, 0f);
        coinsCollected = new SerializableDictionary<string, bool>();
        berryCollected = new SerializableDictionary<string, bool>();
    }
    
}
