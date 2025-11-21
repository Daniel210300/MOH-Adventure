using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ObjetoInteractuable : MonoBehaviour
{
    public string idObjeto;   // Debe coincidir con el patrón del PatronManager
    
    [Header("Efectos Visuales")]
    public Color colorCorrecto = new Color(0f, 1f, 1f);
    public Color colorCompletado = Color.green;
    public float intensidadEmision = 1.0f;
    
    private bool jugadorCerca = false;
    private bool estaSeleccionado = false;
    private bool patronCompletado = false;
    private Material materialOriginal;
    private Color colorOriginal;
    private Renderer objetoRenderer;
    private Coroutine corrutinaColor;
    
    void Start()
    {
        // Obtener el componente Renderer y guardar el material original
        objetoRenderer = GetComponent<Renderer>();
        if (objetoRenderer != null)
        {
            materialOriginal = objetoRenderer.material;
            colorOriginal = objetoRenderer.material.color;
        }
        
        // Suscribirse a los eventos del PatronManager
        if (PatronManager.instance != null)
        {
            PatronManager.instance.OnPatronReiniciado += ReiniciarObjeto;
            PatronManager.instance.OnPatronCompletado += ManejarPatronCompletado;
        }
    }
    
    void OnDestroy()
    {
        // Desuscribirse de los eventos para evitar memory leaks
        if (PatronManager.instance != null)
        {
            PatronManager.instance.OnPatronReiniciado -= ReiniciarObjeto;
            PatronManager.instance.OnPatronCompletado -= ManejarPatronCompletado;
        }
    }

    void Update()
    {
        if (jugadorCerca && Keyboard.current.eKey.wasPressedThisFrame && !estaSeleccionado && !patronCompletado)
        {
            // Verificar si este es el último objeto del patrón
            bool esUltimoObjeto = EsUltimoObjetoDelPatron();
            bool correcto = PatronManager.instance.SeleccionarObjeto(idObjeto);

            if (correcto)
            {
                Debug.Log("✔ Seleccionaste el objeto correcto: " + idObjeto);
                estaSeleccionado = true;
                
                if (esUltimoObjeto)
                {
                    // Si es el último objeto, directamente usar color verde
                    IluminarObjeto(colorCompletado);
                }
                else
                {
                    // Si no es el último, usar color amarillo
                    IluminarObjeto(colorCorrecto);
                }
            }
            else
            {
                Debug.Log("❌ Incorrecto, se reinicia el patrón.");
                // El reinicio se manejará automáticamente a través del evento
            }
        }
    }

    // Método para verificar si este objeto es el último del patrón
    private bool EsUltimoObjetoDelPatron()
    {
        if (PatronManager.instance == null) return false;
        
        string[] patron = PatronManager.instance.patron;
        int indiceActual = PatronManager.instance.GetIndiceActual();
        
        // Si el índice actual es el último (longitud - 1) y este objeto coincide con el último elemento
        return indiceActual == patron.Length - 1 && idObjeto == patron[patron.Length - 1];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !estaSeleccionado && !patronCompletado)
        {
            jugadorCerca = true;
            if (UIManager.instance != null)
                UIManager.instance.MostrarMensaje("Presiona E para seleccionar");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (UIManager.instance != null)
                UIManager.instance.OcultarMensaje();
        }
    }
    
    // Método para iluminar el objeto
    private void IluminarObjeto(Color color)
    {
        if (objetoRenderer != null)
        {
            // Detener corrutina anterior si existe
            if (corrutinaColor != null)
                StopCoroutine(corrutinaColor);
            
            // Cambiar el color
            objetoRenderer.material.color = color;
            
            // Activar emisión para mejor efecto
            objetoRenderer.material.EnableKeyword("_EMISSION");
            objetoRenderer.material.SetColor("_EmissionColor", color * intensidadEmision);
        }
    }
    
    // Método para apagar el objeto
    private void ApagarObjeto()
    {
        if (objetoRenderer != null)
        {
            // Detener corrutina anterior si existe
            if (corrutinaColor != null)
                StopCoroutine(corrutinaColor);
            
            // Restaurar el color original
            objetoRenderer.material.color = colorOriginal;
            
            // Apagar la emisión
            objetoRenderer.material.DisableKeyword("_EMISSION");
        }
    }
    
    // Método llamado cuando se reinicia el patrón
    private void ReiniciarObjeto()
    {
        estaSeleccionado = false;
        patronCompletado = false;
        ApagarObjeto();
    }
    
    // Método para manejar cuando se completa el patrón
    private void ManejarPatronCompletado()
    {
        patronCompletado = true;
        
        // Cambiar a color verde cuando se completa el patrón
        if (estaSeleccionado && objetoRenderer != null)
        {
            IluminarObjeto(colorCompletado);
        }
    }

    // Método para debug visual
    void OnDrawGizmos()
    {
        if (jugadorCerca)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 1.5f);
        }
    }
}