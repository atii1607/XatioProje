using UnityEngine;

public class RestartGame : MonoBehaviour
{
    [SerializeField] private GameObject restartGame;
    [SerializeField] private PlayerMovement playerMovement;

    private LivesCountText livesCountText;

    private void Start()
    {
        if (restartGame != null)
        {
            restartGame.SetActive(false);
        }
        livesCountText = FindObjectOfType<LivesCountText>();

        if (DataPersistenceManager.instance.gameData != null && DataPersistenceManager.instance.gameData.playerLives <= 0)
        {
            RestartScreen();
            playerMovement.isAlive = false;
            playerMovement.moveInput = Vector2.zero;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies"), true);
        }
    }

    public void RestartScreen()
    {
        restartGame.SetActive(true);
    }

    public void OnRestartButtonClicked()
    {
        livesCountText.isDead = false;
        restartGame.SetActive(false);

        livesCountText.RespawnLife();
        playerMovement.ResetPlayer(DataPersistenceManager.instance.gameData.respawnPosition);
    }

}
