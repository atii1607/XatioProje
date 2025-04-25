using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryPickup : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;
    public float boostSpeed = 10f;
    public float jumpBoost = 10f;
    public float duration = 5f;
    private bool collected = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CollectRedBerry(collision.GetComponent<PlayerMovement>());
            CollectYellowBerry(collision.GetComponent<PlayerMovement>());
        }
    }
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public void LoadData(GameData gameData)
    {
        gameData.berryCollected.TryGetValue(id, out collected);
        if (collected)
        {
            Destroy(gameObject);
        }
    }
    public void SaveData(GameData gameData)
    {
        if (gameData.berryCollected.ContainsKey(id))
        {
            gameData.berryCollected.Remove(id);
        }
        gameData.berryCollected.Add(id, collected);
    }

    private void CollectRedBerry(PlayerMovement player)
    {
        if (player != null)
        {
            jumpBoost = 5f;
            collected = true;
            player.ActivateSpeedBoost(boostSpeed, duration);
            Destroy(gameObject);
        }
    }
    private void CollectYellowBerry(PlayerMovement player)
    {
        if (player != null)
        {
            boostSpeed = 5f;
            collected = true;
            player.ActivateJumpBoost(jumpBoost, duration);
            Destroy(gameObject);
        }
    }

}
