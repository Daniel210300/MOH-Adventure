using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzlePieceManager : MonoBehaviour
{
    [Header("Door")]
    public DoorRamas doorRamas;   // ← referencia a la puerta

    public static PuzzlePieceManager instance;

    [Header("Configuración")]
    public int totalPieces = 3;
    private int collectedPieces = 0;

    [Header("HUD")]
    public HUDObjectives hudObjectives; // << referencia al texto del HUD
    public string nextLevelName = "Nivel2";

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateHUD();
    }

    public void CollectPiece()
    {
        collectedPieces++;
        collectedPieces = Mathf.Clamp(collectedPieces, 0, totalPieces);

        // 🔵 Mostrar objetivos en HUD
        UpdateHUD();

        // 🔵 DIÁLOGO DE LUMINA AL AGARRAR EL PRIMER CRISTAL
        if (collectedPieces == 1)
        {
            LuminaSubtitleSystem subtitle = FindFirstObjectByType<LuminaSubtitleSystem>();
            if (subtitle != null)
            {
                subtitle.LuminaDice(
                    "Excelente! Has recogido el primer cristal. Pero para llegar a la cueva necesitamos recolectar los tres de Sello que protegen la entrada",
                    8f
                );
            }
        }

        if (collectedPieces >= totalPieces)
        {
            Debug.Log("Todas las piezas recolectadas!");
        }
        if (collectedPieces >= totalPieces)
        {
            Debug.Log("Todas las piezas recolectadas, abriendo la puerta!");
            if (doorRamas != null)
                doorRamas.OpenDoor();
        }

    }

    void UpdateHUD()
    {
        if (hudObjectives != null)
        {
            hudObjectives.SetObjectiveText($"Cristales: {collectedPieces}/{totalPieces}");
        }
    }

    public bool AreAllPiecesCollected()
    {
        return collectedPieces >= totalPieces;
    }
}
