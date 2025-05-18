using System.Collections;
using UnityEngine;
using TMPro;

public class LivesCountText : MonoBehaviour, IDataPersistence
{
    [SerializeField] private PlayerMovement playerMovement;

    private TextMeshProUGUI livesCountText;
    private RestartGame restartGame;

    private int playerLives;
    public bool isDead = false;

    public void LoadData(GameData gameData)
    {
        this.playerLives = gameData.playerLives;
    }

    public void SaveData(GameData gameData)
    {
        gameData.playerLives = this.playerLives;
    }

    private void Start()
    {
        restartGame = FindObjectOfType<RestartGame>();
        GameObject livesTextObject = GameObject.Find("Lives Text");

        if (livesTextObject != null)
        {
            livesCountText = livesTextObject.GetComponent<TextMeshProUGUI>();
        }

        UpdateLivesDisplay();
    }

    private void UpdateLivesDisplay()
    {
        if (livesCountText != null)
        {
            livesCountText.text = "Lives: " + playerLives;
        }
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
            restartGame.RestartScreen();
        }
    }

    private IEnumerator RespawnPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerMovement.ResetPlayer(DataPersistenceManager.instance.gameData.respawnPosition);
    }
}
