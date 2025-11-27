using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject pauseMenu;
    public PlayerInput playerInput; // arraste o PlayerInput do player (ou deixe vazio para buscar)

    private InputAction pauseAction;
    private bool isPaused = false;

    [System.Obsolete]
    void Awake()
    {
        if (playerInput == null)
        {
            playerInput = FindObjectOfType<PlayerInput>(); // pega o primeiro PlayerInput da cena
        }

        if (playerInput == null)
            Debug.LogError("PauseController: PlayerInput não encontrado. Arraste manualmente no Inspector.");

        if (pauseMenu != null) pauseMenu.SetActive(false);

        // tenta pegar a action (pode lançar se não existir)
        if (playerInput != null)
            pauseAction = playerInput.actions["Pause"];
    }

    void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.Enable();
            pauseAction.performed += OnPausePerformed;
        }
    }

    void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.performed -= OnPausePerformed;
            pauseAction.Disable();
        }
    }

    private void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        TogglePause();
    }

    public void TogglePause()
    {
        if (isPaused) ResumeGame();
        else PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (pauseMenu) pauseMenu.SetActive(true);
        isPaused = true;

        // opcional: trocar ActionMap para UI se você tiver
        if (playerInput != null) playerInput.SwitchCurrentActionMap("UI");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (pauseMenu) pauseMenu.SetActive(false);
        isPaused = false;
        if (playerInput != null) playerInput.SwitchCurrentActionMap("Player");
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        if (pauseMenu) pauseMenu.SetActive(false);
        isPaused = false;
        if (playerInput != null) playerInput.SwitchCurrentActionMap("Player");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
