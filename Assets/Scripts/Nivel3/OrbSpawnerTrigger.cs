using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // Necesario si usas TextMeshPro

public class OrbSpawnerTrigger : MonoBehaviour
{
    public LightCollector collector;       // Script de Moh
    public Transform barronusTarget;       // Empty dentro de Barronus
    public TextMeshProUGUI pressFText;    // Texto del Canvas

    private bool playerInRange = false;

    void Start()
    {
        if (pressFText != null)
            pressFText.gameObject.SetActive(false); // Oculto al inicio
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (pressFText != null)
                pressFText.gameObject.SetActive(true); // Mostrar texto
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (pressFText != null)
                pressFText.gameObject.SetActive(false); // Ocultar texto
        }
    }

    void Update()
    {
        if (!playerInRange) return;       
        if (!collector.HasOrbs()) return; 

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            collector.LaunchOrb(barronusTarget);
        }
    }
}
