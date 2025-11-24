using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryZone : MonoBehaviour
{
    public BarronusHitReceiver barronus; // solo para Nivel3

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Verificar si el jugador ha recogido todas las piezas
        if (PuzzlePieceManager.instance != null && !PuzzlePieceManager.instance.AreAllPiecesCollected())
        {
            Debug.Log("Aún no has recogido todas las piezas.");
            return;
        }

        string currentScene = SceneManager.GetActiveScene().name;

        // Nivel3: revisar que Barronus esté muerto
        if (currentScene == "Nivel3")
        {
            if (barronus != null && !barronus.isDead)
            {
                Debug.Log("Barronus aún no ha sido derrotado.");
                return;
            }
        }

        // Mostrar pantalla de nivel completado
        if (LevelCompleteManager.Instance != null)
            LevelCompleteManager.Instance.ShowLevelComplete();
    }
}
