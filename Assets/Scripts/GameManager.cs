using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI de Game Over")]
    public GameObject gameOverPanel;
    public Button retryButton;
    public Button quitButton;
    public Text gameOverText;

    private PlayerEnergy playerEnergy;

    void Start()
    {
        // Encontrar el PlayerEnergy
        playerEnergy = FindFirstObjectByType<PlayerEnergy>();
        
        if (playerEnergy != null)
        {
            playerEnergy.onPlayerDeath += ShowGameOverScreen;
        }

        // Configurar botones
        if (retryButton != null)
            retryButton.onClick.AddListener(RetryGame);
        
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

        // Ocultar panel al inicio
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void ShowGameOverScreen()
    {
        // Mostrar el panel de Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Pausar el juego (opcional)
        Time.timeScale = 0f;
    }

    void RetryGame()
    {
        // Reanudar el tiempo primero
        Time.timeScale = 1f;
        
        // Ocultar TODOS los Canvas antes de recargar
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        // Ocultar Canvas de LevelManager si existe
        if (LevelManager.Instance != null && LevelManager.Instance.levelCompleteUI != null)
        {
            LevelManager.Instance.levelCompleteUI.SetActive(false);
        }
        
        // Recargar la escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void QuitGame()
    {
        // Salir del juego (en build) o del editor
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void OnDestroy()
    {
        // Limpiar suscripciones
        if (playerEnergy != null)
            playerEnergy.onPlayerDeath -= ShowGameOverScreen;
    }
}