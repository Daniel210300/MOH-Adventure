using UnityEngine;

public class FloatingPuzzlePiece : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float floatHeight = 0.5f;
    
    private Vector3 startPosition;
    private bool collected = false;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (!collected)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;
            
            // Notificar al manager de coleccionables
            if (PuzzlePieceManager.instance != null)
            {
                PuzzlePieceManager.instance.CollectPiece();
            }
            
            // Efectos visuales/sonoros opcionales aquí
            // Ejemplo: partículas, sonido, etc.
            
            // Destruir el objeto
            Destroy(gameObject);
        }
    }
}