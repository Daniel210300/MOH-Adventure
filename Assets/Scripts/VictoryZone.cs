using UnityEngine;

public class VictoryZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Verificar si el jugador tiene todas las piezas
            if (PuzzlePieceManager.instance != null && 
                PuzzlePieceManager.instance.AreAllPiecesCollected())
            {
                Debug.Log("¡Victoria! Jugador en la cueva con todas las piezas");
                PuzzlePieceManager.instance.ShowVictoryUI();
            }
            else
            {
                Debug.Log("Entraste en la cueva, pero te faltan piezas");
                // Opcional: Mostrar mensaje al jugador
            }
        }
    }
}