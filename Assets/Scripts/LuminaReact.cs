using UnityEngine;

public class LuminaReact : MonoBehaviour
{
    public ParticleSystem glowBurst; // efecto de partículas breve
    public Light luminaLight; // la luz principal de Lumina
    public Color reactColor = Color.cyan;
    public float flashIntensity = 6f;
    public float flashDuration = 0.5f;

    private Color originalColor;
    private float originalIntensity;

    void Start()
    {
        if (luminaLight != null)
        {
            originalColor = luminaLight.color;
            originalIntensity = luminaLight.intensity;
        }
    }

    public void React()
    {
        if (glowBurst != null)
            glowBurst.Play();

        if (luminaLight != null)
            StartCoroutine(FlashLight());
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
