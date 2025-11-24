using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup pauseGroup;
    public Button resumeButton;
    public Button retryButton;
    public Button quitButton;

    private bool isPaused = false;

    void Start()
    {
        // Inicia invisible
        pauseGroup.alpha = 0;
        pauseGroup.interactable = false;
        pauseGroup.blocksRaycasts = false;

        // Botones
        resumeButton.onClick.AddListener(ResumeGame);
        retryButton.onClick.AddListener(RetryLevel);
        quitButton.onClick.AddListener(QuitToMenu);
    }

    void Update()
    {
        // Toggle pausa con ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        pauseGroup.alpha = 1;
        pauseGroup.interactable = true;
        pauseGroup.blocksRaycasts = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        pauseGroup.alpha = 0;
        pauseGroup.interactable = false;
        pauseGroup.blocksRaycasts = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Cambia si tu menu tiene otro nombre
    }
}
