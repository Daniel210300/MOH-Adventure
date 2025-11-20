using UnityEngine;
using UnityEngine.UI;

public class LuminaEnergy : MonoBehaviour
{
    public float maxEnergy = 100f;
    public float currentEnergy;
    public float drainPerSecondWhenActive = 5f;
    public Light luminaLight;
    public Slider energySlider;
    public float minIntensity = 0.3f;
    public float maxIntensity = 3f;

    void Start()
    {
        currentEnergy = maxEnergy;
        if (energySlider) { energySlider.maxValue = maxEnergy; energySlider.value = currentEnergy; }
    }

    void Update()
    {
        // ejemplo: si Lumina está "activada" (puedes condicionar con un bool), drena
        // aquí asumimos que Lumina siempre consume si la intensidad > min
        if (luminaLight != null && luminaLight.intensity > minIntensity)
        {
            currentEnergy -= drainPerSecondWhenActive * Time.deltaTime;
            if (currentEnergy < 0) currentEnergy = 0;
            UpdateLight();
        }
        if (energySlider) energySlider.value = currentEnergy;
    }

    void UpdateLight()
    {
        if (luminaLight)
        {
            float t = currentEnergy / maxEnergy;
            luminaLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
        }
    }

    public void RestoreEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0f, maxEnergy);
        UpdateLight();
        if (energySlider) energySlider.value = currentEnergy;
    }

    // llamado al resolver secuencia con recompensa
    void OnSequenceSolved()
    {
        RestoreEnergy(maxEnergy * 0.25f); // premio
    }
}
