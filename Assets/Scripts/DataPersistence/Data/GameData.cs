using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public int playerLives;
    public int playerCoins;
    public Vector3 playerPosition;
    public SerializableDictionary<string, bool> coinsCollected;

    public GameData()
    {
        this.playerLives = 3;
        this.playerCoins = 0;
        playerPosition = new Vector3(-10.49f, -5.09f, 0f);
        coinsCollected = new SerializableDictionary<string, bool>();
    }
    
}
