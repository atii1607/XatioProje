using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BerryPickup;

public class BerryPickup : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;
    private float boostSpeed = 8f;
    private float jumpBoost = 1.3f;
    private float duration = 5f;
    private bool collected = false;
    public enum BerryType
    {
        RedBerry,
        YellowBerry,
        GreenBerry,
    }
    public BerryType berryType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collected)
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            CollectBerry(player);
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

    private void CollectBerry(PlayerMovement player)
    {
        if (player != null)
        {
            collected = true;
            ApplyBerryEffect(player);
            Destroy(gameObject);
        }
    }

    private void ApplyBerryEffect(PlayerMovement player)
    {
        switch (berryType)
        {
            case BerryType.RedBerry:
                player.ActivateSpeedBoost(boostSpeed, duration);
                break;

            case BerryType.YellowBerry:
                player.ActivateJumpBoost(jumpBoost, duration);
                break;

            case BerryType.GreenBerry:
                player.ActivateInvincibility(duration);
                break;

            default:
                Debug.LogWarning("Berry type not handled!");
                break;
        }
    }

}
