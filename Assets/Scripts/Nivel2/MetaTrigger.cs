using UnityEngine;

public class MetaTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verificar si es el jugador quien entra en la meta
        if (other.CompareTag("Player"))
        {
            // Llamar al LevelManager para completar el nivel
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.LevelCompleted();
            }
            else
            {
                Debug.LogError("LevelManager no encontrado!");
            }
        }
    }
}