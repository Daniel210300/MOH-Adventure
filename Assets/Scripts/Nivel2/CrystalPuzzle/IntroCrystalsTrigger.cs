using UnityEngine;
using UnityEngine.InputSystem;

public class IntroCrystalsTrigger : MonoBehaviour
{
    [Header("Crystal Sequence")]
    public Transform[] crystalSequence;
    public float flashDuration = 0.5f;
    public float delayBetweenFlashes = 0.3f;
    public Color glowColor = Color.cyan; // Color del brillo
    public float glowIntensity = 2f;     // Intensidad del brillo

    private bool playerInside = false;
    private bool isPlayingSequence = false;
    private Material[] originalMaterials; // Para guardar los materiales originales

    void Start()
    {
        // Guardar los materiales originales de los cristales
        originalMaterials = new Material[crystalSequence.Length];
        for (int i = 0; i < crystalSequence.Length; i++)
        {
            if (crystalSequence[i] != null)
            {
                Renderer rend = crystalSequence[i].GetComponent<Renderer>();
                if (rend != null)
                {
                    originalMaterials[i] = rend.material;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            if (InteractionUI.instance != null)
                InteractionUI.instance.Show("Presiona E para ver la combinación");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            if (InteractionUI.instance != null)
                InteractionUI.instance.Hide();
        }
    }

    void Update()
    {
        if (playerInside && Keyboard.current.eKey.wasPressedThisFrame && !isPlayingSequence)
        {
            if (InteractionUI.instance != null)
                InteractionUI.instance.Hide();
                
            StartCoroutine(PlaySequence());
        }
    }

    private System.Collections.IEnumerator PlaySequence()
    {
        isPlayingSequence = true;

        // Secuencia de luces en los cristales con emisión
        foreach (Transform crystal in crystalSequence)
        {
            if (crystal == null) continue;
            
            Renderer rend = crystal.GetComponent<Renderer>();
            if (rend != null)
            {
                // Activar emisión
                yield return StartCoroutine(GlowCrystal(rend));
                yield return new WaitForSeconds(delayBetweenFlashes);
            }
        }

        isPlayingSequence = false;
        
        // Mostrar UI nuevamente si el jugador sigue en el trigger
        if (playerInside && InteractionUI.instance != null)
        {
            InteractionUI.instance.Show("Presiona E para ver la combinación");
        }
    }

    private System.Collections.IEnumerator GlowCrystal(Renderer rend)
    {
        Material mat = rend.material;
        
        // Asegurar que el material tenga emisión
        if (!mat.HasProperty("_EmissionColor"))
        {
            // Si no tiene propiedad de emisión, usar el método alternativo
            yield return StartCoroutine(GlowCrystalAlternative(rend));
            yield break;
        }

        // Guardar valores originales
        Color originalEmission = mat.GetColor("_EmissionColor");
        bool originalEmissionState = mat.IsKeywordEnabled("_EMISSION");

        // Activar emisión
        mat.EnableKeyword("_EMISSION");
        
        float flashTime = 0f;
        while (flashTime < flashDuration)
        {
            flashTime += Time.deltaTime;
            float intensity = Mathf.Sin((flashTime / flashDuration) * Mathf.PI) * glowIntensity;
            Color emissionColor = glowColor * intensity;
            mat.SetColor("_EmissionColor", emissionColor);
            
            // Forzar la actualización de la luz global (importante para GI)
            // DynamicGI.SetEmissive(rend, emissionColor);
            
            yield return null;
        }

        // Restaurar valores originales
        mat.SetColor("_EmissionColor", originalEmission);
        if (!originalEmissionState)
            mat.DisableKeyword("_EMISSION");
    }

    // Método alternativo si el material no soporta emisión
    private System.Collections.IEnumerator GlowCrystalAlternative(Renderer rend)
    {
        Material mat = rend.material;
        Color originalColor = mat.color;
        Color glowColor = this.glowColor * 2f; // Más brillante para compensar

        float flashTime = 0f;
        while (flashTime < flashDuration)
        {
            flashTime += Time.deltaTime;
            float intensity = Mathf.Sin((flashTime / flashDuration) * Mathf.PI);
            mat.color = Color.Lerp(originalColor, glowColor, intensity);
            yield return null;
        }

        mat.color = originalColor;
    }
}