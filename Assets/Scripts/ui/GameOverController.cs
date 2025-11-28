using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOverPanel;
    public MusicManager musicManager;

    private PlayerInput playerInput;

    [System.Obsolete]
    void Awake()
    {
        if (gameOverPanel) 
            gameOverPanel.SetActive(false);

        playerInput = FindObjectOfType<PlayerInput>();
    }

    public void TriggerGameOver()
    {
        Time.timeScale = 0f;
        LevelDirector.GamePaused = true;
        LevelDirector.Instance.OnPause();

        if (musicManager != null)
            musicManager.PauseMusic();

        if (gameOverPanel)
            gameOverPanel.SetActive(true);

        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("UI");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        LevelDirector.GamePaused = false;
        LevelDirector.Instance.OnResume();

        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("Player");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        LevelDirector.GamePaused = false;
        LevelDirector.Instance.OnResume();

        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("Player");

        SceneManager.LoadScene(0);
    }
}
