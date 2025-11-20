using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzlePieceManager : MonoBehaviour
{
    public static PuzzlePieceManager instance;
    
    [Header("UI Elements")]
    public GameObject collectionUI;
    public TextMeshProUGUI counterText;
    public GameObject levelCompleteUI;
    
    [Header("Configuración")]
    public int totalPieces = 3;
    public string nextLevelName = "Nivel2";
    
    private int collectedPieces = 0;
    private bool isUIVisible = false;
    private bool allPiecesCollected = false;
    
    // Objetos que se moverán cuando se completen las piezas
    [Header("Objetos que se desplazarán")]
    public Transform[] objectsToMove;
    public Vector3[] targetPositions;
    public float moveSpeed = 2f;
    
    private bool objectsMoving = false;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    void Start()
    {
        InitializeUI();
        UpdateCounterText();
    }
    
    void Update()
    {
        // Mostrar/ocultar UI con TAB
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleUI();
        }
        
        // Mover objetos si es necesario
        if (objectsMoving)
        {
            MoveObjects();
        }
    }
    
    private void InitializeUI()
    {
        // Ocultar UI al inicio
        if (collectionUI != null)
        {
            collectionUI.SetActive(false);
        }
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(false);
        }
    }
    
    public void CollectPiece()
    {
        collectedPieces++;
        collectedPieces = Mathf.Clamp(collectedPieces, 0, totalPieces);
        
        UpdateCounterText();
        
        Debug.Log($"Pieza coleccionada: {collectedPieces}/{totalPieces}");
        
        if (collectedPieces >= totalPieces && !allPiecesCollected)
        {
            AllPiecesCollected();
        }
    }
    
    private void UpdateCounterText()
    {
        if (counterText != null)
        {
            counterText.text = $"{collectedPieces} de {totalPieces}";
        }
    }
    
    private void ToggleUI()
    {
        isUIVisible = !isUIVisible;
        
        if (collectionUI != null)
        {
            collectionUI.SetActive(isUIVisible);
        }
    }
    
    private void AllPiecesCollected()
    {
        allPiecesCollected = true;
        Debug.Log("¡Todas las piezas han sido recolectadas!");
        
        // Iniciar movimiento de objetos
        if (objectsToMove != null && objectsToMove.Length > 0)
        {
            objectsMoving = true;
        }
        
        // No mostrar UI de victoria aquí, solo cuando llegue a la cueva
    }
    
    // NUEVO MÉTODO: Mostrar UI cuando llegue a la cueva con todas las piezas
    public void ShowVictoryUI()
    {
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(true);
            
            // Opcional: Pausar el juego
            Time.timeScale = 0f;
            
            // Mostrar cursor para poder hacer clic en los botones
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    private void MoveObjects()
    {
        bool allObjectsInPosition = true;
        
        for (int i = 0; i < objectsToMove.Length; i++)
        {
            if (i < targetPositions.Length && objectsToMove[i] != null)
            {
                objectsToMove[i].position = Vector3.MoveTowards(
                    objectsToMove[i].position, 
                    targetPositions[i], 
                    moveSpeed * Time.deltaTime
                );
                
                if (Vector3.Distance(objectsToMove[i].position, targetPositions[i]) > 0.01f)
                {
                    allObjectsInPosition = false;
                }
            }
        }
        
        if (allObjectsInPosition)
        {
            objectsMoving = false;
            Debug.Log("Todos los objetos han llegado a su posición");
        }
    }
    
    public void GoToNextLevel()
    {
        // Reanudar juego si estaba pausado
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Reiniciar contador
        collectedPieces = 0;
        allPiecesCollected = false;
        UpdateCounterText();
        
        // Ocultar UI
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(false);
        }
        
        // Cargar siguiente nivel
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            SceneManager.LoadScene(nextLevelName);
        }
    }
    
    public void RestartLevel()
    {
        // Reanudar juego si estaba pausado
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        collectedPieces = 0;
        allPiecesCollected = false;
        UpdateCounterText();
        
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(false);
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    
    public int GetCollectedPieces() => collectedPieces;
    public int GetTotalPieces() => totalPieces;
    public bool AreAllPiecesCollected() => allPiecesCollected;
}