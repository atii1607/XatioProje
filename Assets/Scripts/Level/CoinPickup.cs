using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CoinPickup : MonoBehaviour, IDataPersistence
{
    [SerializeField] AudioClip coinPickupSound;
    [SerializeField] private string id;
    private bool collected = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            FindObjectOfType<LevelCountText>().CollectCoin();
            AudioSource.PlayClipAtPoint(coinPickupSound, Camera.main.transform.position);
            Destroy(gameObject);
            collected = true;
        }
    }
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public void LoadData(GameData gameData)
    {
        gameData.coinsCollected.TryGetValue(id, out collected);
        if (collected)
        {
            Destroy(gameObject);
        }
    }
    public void SaveData(ref GameData gameData)
    {
        if (gameData.coinsCollected.ContainsKey(id))
        {
            gameData.coinsCollected.Remove(id);
        }
        gameData.coinsCollected.Add(id, collected);
    }
}
