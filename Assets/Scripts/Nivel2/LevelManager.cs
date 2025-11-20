using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Singleton para acceder fácilmente desde cualquier script
    public static LevelManager Instance { get; private set; }
    
    // Nombres de las escenas (ya los tienes definidos)
    public string mainMenuScene = "MainMenu";
    public string nextLevelScene = "Nivel3";
    
    // Referencia al Canvas de UI que ya tienes
    public GameObject levelCompleteUI;
    
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
        }
        
        // Asegurarse de que la UI esté oculta al inicio
        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(false);
    }
    
    // Método para cuando el jugador alcanza la meta
    public void LevelCompleted()
    {
        // Mostrar la UI de nivel completado
        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(true);
        
        // Pausar el juego (opcional)
        Time.timeScale = 0f;
    }
    
    // Métodos para los botones de la UI
    public void LoadNextLevel()
    {
        // Reanudar el tiempo si estaba pausado
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextLevelScene);
    }
    
    public void LoadMainMenu()
    {
        // Reanudar el tiempo si estaba pausado
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
    
    public void RestartLevel()
    {
        // Reanudar el tiempo si estaba pausado
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}