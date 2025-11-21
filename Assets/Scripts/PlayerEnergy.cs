using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        string escenaActual = SceneManager.GetActiveScene().name;

        // 🚫 Niveles donde NO queremos GameOver
        if (escenaActual == "Nivel1")
        {
            Debug.Log("⚠ El personaje murió, pero en Nivel1 NO se ejecuta GameOver.");
            return;
        }

        // ✔ Nivel2 - usa LevelManager original
        if (escenaActual == "Nivel2")
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.GameOver();
            }
            else
            {
                Debug.LogError("LevelManager.Instance no encontrada");
            }
        }

        // ✔ Nivel3 - usa LevelManager3
        if (escenaActual == "Nivel3")
        {
            if (LevelManager3.Instance != null)
            {
                LevelManager3.Instance.GameOver();
            }
            else
            {
                Debug.LogError("LevelManager3.Instance no encontrada");
            }
        }
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
        if (isDead) return;

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

    // Método para revivir al jugador (si necesitas resetear)
    public void Revive()
    {
        isDead = false;
        currentEnergy = maxEnergy;
        UpdateEnergyBar();
    }
}