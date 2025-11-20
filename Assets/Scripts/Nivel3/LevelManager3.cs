using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager3 : MonoBehaviour
{
    // Singleton para acceder fácilmente desde cualquier script
    public static LevelManager3 Instance { get; private set; }
    
    // Nombres de las escenas
    public string mainMenuScene = "MainMenu";
    
    // Referencias a los Canvas de UI
    public GameObject levelCompleteUI;
    public GameObject gameOverUI;
    
    private void Awake()
    {
        // Configurar el singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Suscribirse al evento de carga de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // Ocultar UIs inicialmente
        HideAllUI();
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cada vez que se carga una nueva escena, buscar las UIs
        FindAllUI();
        HideAllUI();
        
        // Asegurarse de que el tiempo esté corriendo
        Time.timeScale = 1f;
    }
    
    private void FindAllUI()
    {
        // Buscar LevelComplete UI
        if (levelCompleteUI == null)
        {
            GameObject uiObject = GameObject.FindGameObjectWithTag("LevelCompleteUI");
            if (uiObject != null)
            {
                levelCompleteUI = uiObject;
            }
            else
            {
                uiObject = GameObject.Find("LevelCompleteCanvas");
                if (uiObject != null)
                {
                    levelCompleteUI = uiObject;
                }
            }
        }
        
        // Buscar GameOver UI
        if (gameOverUI == null)
        {
            GameObject uiObject = GameObject.FindGameObjectWithTag("GameOverUI");
            if (uiObject != null)
            {
                gameOverUI = uiObject;
            }
            else
            {
                uiObject = GameObject.Find("GameOverCanvas");
                if (uiObject != null)
                {
                    gameOverUI = uiObject;
                }
            }
        }
    }
    
    private void HideAllUI()
    {
        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(false);
            
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }
    
    // Método para cuando el jugador completa el nivel
    public void LevelCompleted()
    {
        // Asegurarse de que tenemos las referencias actualizadas
        FindAllUI();
        
        // Mostrar la UI de nivel completado y ocultar otras
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(true);
        }
        
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
        
        // Pausar el juego (opcional)
        Time.timeScale = 0f;
    }
    
    // Método para cuando el jugador muere
    public void GameOver()
    {
        // Asegurarse de que tenemos las referencias actualizadas
        FindAllUI();
        
        // Mostrar la UI de Game Over y ocultar otras
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(false);
        }
        
        // Pausar el juego (opcional)
        Time.timeScale = 0f;
    }
    
    // Métodos para los botones de la UI
    
    
    public void LoadMainMenu()
    {
        // Reanudar el tiempo si estaba pausado
        Time.timeScale = 1f;
        
        // Limpiar las referencias al cambiar al menú principal
        levelCompleteUI = null;
        gameOverUI = null;
        SceneManager.LoadScene(mainMenuScene);
    }
    
    public void RestartLevel()
    {
        // Reanudar el tiempo si estaba pausado
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // Método para continuar desde el Game Over (si aplica)
    public void ContinueFromGameOver()
    {
        // Reanudar el tiempo
        Time.timeScale = 1f;
        
        // Aquí puedes agregar lógica adicional como:
        // - Restaurar vidas del jugador
        // - Reposicionar al jugador
        // - Restablecer enemigos, etc.
        
        // Ocultar UI de Game Over
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
    }
    
    private void OnDestroy()
    {
        // Desuscribirse del evento para evitar memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}