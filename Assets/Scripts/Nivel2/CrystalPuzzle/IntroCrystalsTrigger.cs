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
        // Clonar y guardar los materiales originales de los cristales
        originalMaterials = new Material[crystalSequence.Length];
        for (int i = 0; i < crystalSequence.Length; i++)
        {
            if (crystalSequence[i] != null)
            {
                Renderer rend = crystalSequence[i].GetComponent<Renderer>();
                if (rend != null)
                {
                    // Clonar material solo una vez para cada cristal
                    originalMaterials[i] = new Material(rend.sharedMaterial);
                    rend.material = originalMaterials[i];
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
                InteractionUI.instance.Show("Presiona E para ver");
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
        for (int i = 0; i < crystalSequence.Length; i++)
        {
            Transform crystal = crystalSequence[i];
            if (crystal == null) continue;
            
            Renderer rend = crystal.GetComponent<Renderer>();
            if (rend != null)
            {
                // Activar emisión usando el material ya guardado
                yield return StartCoroutine(GlowCrystal(rend, originalMaterials[i]));
                yield return new WaitForSeconds(delayBetweenFlashes);
            }
        }

        isPlayingSequence = false;
        
        // Mostrar UI nuevamente si el jugador sigue en el trigger
        if (playerInside && InteractionUI.instance != null)
        {
            InteractionUI.instance.Show("Presiona E para ver");
        }
    }

    private System.Collections.IEnumerator GlowCrystal(Renderer rend, Material mat)
    {
        // Asegurar que el material tenga emisión
        if (!mat.HasProperty("_EmissionColor"))
        {
            yield return StartCoroutine(GlowCrystalAlternative(rend, mat));
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
            yield return null;
        }

        // Restaurar valores originales
        mat.SetColor("_EmissionColor", originalEmission);
        if (!originalEmissionState)
            mat.DisableKeyword("_EMISSION");
    }

    private System.Collections.IEnumerator GlowCrystalAlternative(Renderer rend, Material mat)
    {
        Color originalColor = mat.color;
        Color altGlowColor = glowColor * 2f; // Más brillante para compensar

        float flashTime = 0f;
        while (flashTime < flashDuration)
        {
            flashTime += Time.deltaTime;
            float intensity = Mathf.Sin((flashTime / flashDuration) * Mathf.PI);
            mat.color = Color.Lerp(originalColor, altGlowColor, intensity);
            yield return null;
        }

        mat.color = originalColor;
    }
}
