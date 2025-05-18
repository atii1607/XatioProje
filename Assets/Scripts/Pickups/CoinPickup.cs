using UnityEngine;

public class CoinPickup : MonoBehaviour, IDataPersistence
{
    [SerializeField] private AudioClip coinPickupSound;
    [SerializeField] private string id;

    private CoinsCollectedText coinsCountText; 
    
    private bool collected = false;

    private void Start()
    {
        coinsCountText = FindObjectOfType<CoinsCollectedText>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collected)
        {
            AudioSource.PlayClipAtPoint(coinPickupSound, Camera.main.transform.position);
            CollectCoin();
        }
    }
    [ContextMenu("Generate guid for id")]

    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public void LoadData(GameData gameData)
    {
        if (gameData.coinsCollected.TryGetValue(id, out collected) && collected)
        {
            Destroy(gameObject);
        }
    }

    public void SaveData(GameData gameData)
    {
        if (gameData.coinsCollected.ContainsKey(id))
        {
            gameData.coinsCollected[id] = collected;
        }

        else
        {
            gameData.coinsCollected.Add(id, collected);
        }
    }

    private void CollectCoin()
    {
        collected = true;
        coinsCountText.CollectCoin();
        Destroy(gameObject);
    }
}
