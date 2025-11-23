using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerEnergy : MonoBehaviour
{
    public static PlayerEnergy Instance;

    [Header("Energía Vital")]
    public float maxEnergy = 100f;
    public float currentEnergy;
    public float drainRate = 8f;
    public float recoverRate = 15f;
    public float safeZoneRecoveryRate = 10f;

    [Header("Luz y detección")]
    public float lightRadius = 8f;

    [Header("UI - Barra de energía")]
    public Image energyFill;

    [Header("Animación")]
    public Animator animator;

    private bool inSafeZone = false;
    private bool inLight = false;
    private bool isDead = false;

    // 👉 Para que otros scripts puedan saber si está muerto
    public bool IsDead => isDead;

    public System.Action onPlayerDeath;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentEnergy = maxEnergy;
        UpdateEnergyBar();
    }

    void Update()
    {
        if (isDead) return; // ⛔ NADA pasa si ya murió

        inLight = IsInLight();

        if (inSafeZone)
        {
            AddEnergy(safeZoneRecoveryRate * Time.deltaTime);
        }
        else if (!inLight)
        {
            TakeDamage(drainRate * Time.deltaTime);
        }
    }

    // ===============================
    //            MUERTE
    // ===============================

    void Die()
    {
        if (isDead) return;

        isDead = true;

        // 🔥 Animación de muerte
        if (animator != null)
            animator.SetTrigger("Death");

        // ⏳ Congela movimiento después de la animación
        StartCoroutine(FreezeAfterDeath());

        // ⏳ Mostrar Game Over después
        StartCoroutine(ShowGameOverDelayed());

        onPlayerDeath?.Invoke();
    }

    private IEnumerator ShowGameOverDelayed()
    {
        yield return new WaitForSeconds(2f); // tiempo para ver la animación

        GameOverManager go = FindFirstObjectByType<GameOverManager>();
        if (go != null)
            go.ShowGameOver("Moh no pudo escapar de la oscuridad...");
    }

    private IEnumerator FreezeAfterDeath()
    {
        yield return new WaitForSeconds(1.5f);

        var controller = GetComponent<PlayerController>();
        if (controller != null)
            controller.canMove = false;
    }

    // ===============================
    //        ENERGÍA
    // ===============================

    public void AddEnergy(float amount)
    {
        if (isDead) return; // ⛔ NO PERMITIR ENERGÍA SI YA MURIÓ

        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0, maxEnergy);
        UpdateEnergyBar();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentEnergy = Mathf.Clamp(currentEnergy - amount, 0, maxEnergy);
        UpdateEnergyBar();

        if (currentEnergy <= 0 && !isDead)
            Die();
    }

    public void SetInSafeZone(bool value)
    {
        inSafeZone = value;
    }

    // ===============================
    //      DETECCIÓN DE LUZ
    // ===============================

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

    // ===============================
    //              UI
    // ===============================

    void UpdateEnergyBar()
    {
        if (energyFill == null) return;

        energyFill.fillAmount = currentEnergy / maxEnergy;

        if (inSafeZone)
            energyFill.color = Color.white;
        else if (inLight)
            energyFill.color = Color.yellow;
        else
            energyFill.color = Color.red;
    }

    public void Revive()
    {
        isDead = false;
        currentEnergy = maxEnergy;
        UpdateEnergyBar();
    }
}