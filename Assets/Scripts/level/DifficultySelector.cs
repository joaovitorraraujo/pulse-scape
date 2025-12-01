using UnityEngine;

public class DifficultySelector : MonoBehaviour
{
    public PlayerStats player; 
    public GameObject difficultyPanel; 
    public LevelDirector levelDirector; 

    public MusicManager musicManager;

    void Start()
    {
        // congelar o jogo antes da escolha
        Time.timeScale = 0f;
        difficultyPanel.SetActive(true);

        // impedir música
        if (levelDirector != null)
            levelDirector.enabled = false;
    }

    public void TriggerGameOver()
    {
        Time.timeScale = 0f;
        LevelDirector.GamePaused = true;
        LevelDirector.Instance.OnPause();

        if (musicManager != null)
            musicManager.PauseMusic();

        if (difficultyPanel)
            difficultyPanel.SetActive(true);
    }

    public void SelectEasy()
    {
        SetDifficulty(Difficulty.Easy);
    }

    public void SelectNormal()
    {
        SetDifficulty(Difficulty.Normal);
    }

    public void SelectHard()
    {
        SetDifficulty(Difficulty.Hard);
    }

    void SetDifficulty(Difficulty d)
    {
        player.currentDifficulty = d;

        // reaplicar stats imediatamente
        player.SetupDifficulty();  

        // reativa o level
        if (levelDirector != null)
            levelDirector.enabled = true;

        // esconde UI
        difficultyPanel.SetActive(false);

        // inicia o jogo
        Time.timeScale = 1f; 
    }
}
