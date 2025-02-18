using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
