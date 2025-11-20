using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    public static PlayerEnergy Instance; // Singleton

    [Header("Energía Vital")]
    public float maxEnergy = 100f;
    public float currentEnergy;
    public float drainRate = 10f;
    public float recoverRate = 15f;

    [Header("Recuperación en Zona Segura")]
    public float safeZoneRecoveryRate = 10f;

    [Header("Rango de detección de luz")]
    public float lightRadius = 8f;

    [Header("UI - Barra de Energía")]
    public Image energyFill;

    private bool inSafeZone = false;
    private bool inLight = false;
    private bool isDead = false;

    public System.Action onPlayerDeath;

    void Awake()
    {
        // Inicializamos Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); 
    }

    void Start()
    {
        currentEnergy = maxEnergy;
        UpdateEnergyBar();
    }

    void Update()
    {
        if (isDead) return;

        inLight = IsInLight();

        if (inSafeZone)
        {
            AddEnergy(safeZoneRecoveryRate * Time.deltaTime);
        }
        else if (!inLight && !inSafeZone)
        {
            TakeDamage(drainRate * Time.deltaTime);
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("💀 Moh se ha consumido en la oscuridad...");
        onPlayerDeath?.Invoke();
    }

    void UpdateEnergyBar()
    {
        if (energyFill != null)
        {
            energyFill.fillAmount = currentEnergy / maxEnergy;

            if (inSafeZone)
                energyFill.color = Color.white;
            else if (inLight)
                energyFill.color = Color.yellow;
            else
                energyFill.color = Color.red;
        }
    }

    bool IsInLight()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, lightRadius);
        foreach (Collider hit in hits)
        {
            Light l = hit.GetComponent<Light>();
            if (l != null && l.enabled && l.intensity > 0.5f)
                return true;
        }
        return false;
    }

    public void AddEnergy(float amount)
    {
        float oldEnergy = currentEnergy;
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        UpdateEnergyBar();

        if (currentEnergy != oldEnergy)
            Debug.Log("✨ Moh recupera energía. Energía actual: " + currentEnergy);
    }

    public void SetInSafeZone(bool value)
    {
        inSafeZone = value;
    }

    public void TakeDamage(float amount)
    {
        float oldEnergy = currentEnergy;
        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        UpdateEnergyBar();

        if (currentEnergy != oldEnergy)
            Debug.Log("💔 Moh recibió daño. Energía actual: " + currentEnergy);

        if (currentEnergy <= 0 && !isDead)
        {
            Die();
        }
    }
}
