using UnityEngine;
using System.Collections;


public class LuminaReact : MonoBehaviour
{
    public ParticleSystem glowBurst; // efecto visual
    public Light luminaLight; // luz de Lumina
    public Color reactColor = Color.cyan;
    public float flashIntensity = 6f;
    public float flashDuration = 0.5f;

    private Color originalColor;
    private float originalIntensity;

    private LuminaSubtitleSystem subtitleSystem; 
    private bool hasSpoken = false; // Para que solo hable en el primer fragmento

    void Start()
    {
        // Guardar luz original
        if (luminaLight != null)
        {
            originalColor = luminaLight.color;
            originalIntensity = luminaLight.intensity;
        }

        // Recuperar el sistema de subtítulos
        subtitleSystem = FindFirstObjectByType<LuminaSubtitleSystem>();
    }

    public void React()
    {
        // Efecto de partículas
        if (glowBurst != null)
            glowBurst.Play();

        // Efecto de luz
        if (luminaLight != null)
            StartCoroutine(FlashLight());

        // --- 🎤 Diálogo (solo en el primer fragmento) ---
        if (!hasSpoken && subtitleSystem != null)
        {
            hasSpoken = true;
            StartCoroutine(FragmentoDialogo());
        }

        IEnumerator FragmentoDialogo()
        {
            subtitleSystem.LuminaDice("Bien hecho, Moh. Ese fragmento restaurará tu energía.", 4f);
            yield return new WaitForSeconds(4.5f);

            subtitleSystem.LuminaDice(
                "¡Cuidado, Moh! Esas son ramas de Barronus. Están infectadas con veneno. No las toques, busca siempre el camino despejado.",
                6f
            );
        }

    }

    private System.Collections.IEnumerator FlashLight()
    {
        luminaLight.color = reactColor;
        luminaLight.intensity = flashIntensity;

        yield return new WaitForSeconds(flashDuration);

        luminaLight.color = originalColor;
        luminaLight.intensity = originalIntensity;
    }
}
