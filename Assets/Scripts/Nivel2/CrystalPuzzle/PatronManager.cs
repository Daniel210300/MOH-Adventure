using UnityEngine;
using System;

public class PatronManager : MonoBehaviour
{
    public static PatronManager instance;

    [Header("Patrón correcto (orden de selección)")]
    public string[] patron;

    [Header("Puerta que se abrirá")]
    public Animator doorAnimator;

    private int indiceActual = 0;

    // Eventos para notificar a los objetos
    public event Action OnPatronReiniciado;
    public event Action OnPatronCompletado;

    void Awake()
    {
        instance = this;
    }

    public bool SeleccionarObjeto(string idObjeto)
    {
        if (idObjeto == patron[indiceActual])
        {
            indiceActual++;

            if (indiceActual >= patron.Length)
            {
                Debug.Log("✔ Patrón completado correctamente!");
                // Primero notificar completado para que todos los objetos cambien a verde
                OnPatronCompletado?.Invoke();
                AbrirPuerta();
                return true;
            }

            return true;
        }
        else
        {
            Debug.Log("❌ Objeto incorrecto, reiniciando");
            ReiniciarPatron();
            return false;
        }
    }

    public void ReiniciarPatron()
    {
        indiceActual = 0;
        OnPatronReiniciado?.Invoke();
    }

    void AbrirPuerta()
    {
        if (doorAnimator != null)
        {
            Debug.Log("🔓 Activando puerta (trigger Open)");
            doorAnimator.SetTrigger("Open");
        }
        else
        {
            Debug.LogWarning("⚠ No asignaste la puerta en el PatronManager!");
        }
    }

    // Método público para obtener el índice actual (útil para debug)
    public int GetIndiceActual()
    {
        return indiceActual;
    }
}