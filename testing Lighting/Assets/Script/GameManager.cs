using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    // Reference to your PlayerData ScriptableObject asset
    public PlayerData playerData;

    private void Start()
    {
        CharacterHealth.OnPlayerDeath += GameOver;
    }

    private void OnDestroy()
    {
        CharacterHealth.OnPlayerDeath -= GameOver;
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
    }

    public void RestartScene()
    {     
        Time.timeScale = 1f;
        // Subscribe to the sceneLoaded event to reposition the player after the scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // This callback is invoked after the scene has finished loading.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Unsubscribe so this runs only once per restart.
        SceneManager.sceneLoaded -= OnSceneLoaded;
        // Find the player GameObject by tag.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && playerData != null)
        {
            // Set the player's position to the saved position in the PlayerData ScriptableObject.
            player.transform.position = playerData.savedPosition;
        }
    }
}