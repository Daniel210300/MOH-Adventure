using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PuzzleCollectionUI : MonoBehaviour
{
    [System.Serializable]
    public class PuzzlePiece
    {
        public int id;
        public Sprite collectedSprite;    // Imagen cuando se tiene
        public Sprite shadowSprite;       // Silueta cuando no se tiene
    }
    
    [Header("Configuración")]
    public List<PuzzlePiece> puzzlePieces;
    public GameObject pieceUIPrefab;      // Prefab para cada icono de pieza
    public Transform piecesContainer;     // Donde se instanciarán las piezas UI
    
    [Header("Diseño")]
    public int maxPiecesPerRow = 4;
    public float spacing = 120f;
    
    private Dictionary<int, Image> pieceImages = new Dictionary<int, Image>();
    private HashSet<int> collectedPieces = new HashSet<int>();
    
    void Start()
    {
        InitializeUI();
    }
    
    void InitializeUI()
    {
        // Limpiar contenedor
        foreach (Transform child in piecesContainer)
        {
            Destroy(child.gameObject);
        }
        
        // Crear UI para cada pieza
        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            GameObject pieceUI = Instantiate(pieceUIPrefab, piecesContainer);
            RectTransform rect = pieceUI.GetComponent<RectTransform>();
            
            // Calcular posición en grid
            int row = i / maxPiecesPerRow;
            int col = i % maxPiecesPerRow;
            rect.anchoredPosition = new Vector2(col * spacing, -row * spacing);
            
            // Configurar imagen (inicialmente sombra)
            Image image = pieceUI.GetComponent<Image>();
            image.sprite = puzzlePieces[i].shadowSprite;
            image.color = new Color(0.3f, 0.3f, 0.3f, 0.7f); // Gris semitransparente
            
            // Guardar referencia
            pieceImages[puzzlePieces[i].id] = image;
        }
    }
    
    public void CollectPiece(int pieceID)
    {
        if (collectedPieces.Contains(pieceID)) return;
        
        collectedPieces.Add(pieceID);
        
        // Actualizar UI
        if (pieceImages.ContainsKey(pieceID))
        {
            PuzzlePiece piece = puzzlePieces.Find(p => p.id == pieceID);
            if (piece != null)
            {
                Image image = pieceImages[pieceID];
                image.sprite = piece.collectedSprite;
                image.color = Color.white; // Color normal
                
                // Efecto opcional (animación, escala, etc.)
                StartCoroutine(CollectEffect(image.transform));
            }
        }
        
        Debug.Log("✅ Pieza " + pieceID + " agregada al inventario!");
        
        // Verificar si se completó el rompecabezas
        CheckPuzzleCompletion();
    }
    
    private System.Collections.IEnumerator CollectEffect(Transform pieceTransform)
    {
        Vector3 originalScale = pieceTransform.localScale;
        pieceTransform.localScale = originalScale * 1.3f;
        yield return new WaitForSeconds(0.2f);
        pieceTransform.localScale = originalScale;
    }
    
    void CheckPuzzleCompletion()
    {
        if (collectedPieces.Count == puzzlePieces.Count)
        {
            Debug.Log("🎉 ¡Rompecabezas completado!");
            // Aquí puedes activar una recompensa
        }
    }
}