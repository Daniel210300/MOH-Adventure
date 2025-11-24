using UnityEngine;

public class BarronusAttack : MonoBehaviour
{
    [Header("Spore Settings")]
    public GameObject sporePrefab;
    public Transform sporeSpawnPoint;
    public Transform sporeTargetPoint; // <-- Punto exacto en Moh
    public float attackInterval = 6f;
    public int sporesPerWave = 3;
    public float spreadAngle = 30f;

    [HideInInspector] public bool isDead = false;

    private float timer = 0f;

    void Update()
    {
        if (sporeTargetPoint == null || isDead) return;

        timer += Time.deltaTime;
        if (timer >= attackInterval)
        {
            timer = 0f;
            LaunchSporeWave();
        }
    }

    void LaunchSporeWave()
    {
        if (sporeTargetPoint == null) return;

        Vector3 baseDir = (sporeTargetPoint.position - sporeSpawnPoint.position).normalized;

        if (baseDir == Vector3.zero)
            baseDir = transform.forward; // fallback seguro

        for (int i = 0; i < sporesPerWave; i++)
        {
            // Offset lateral para dispersión
            float offset = Mathf.Lerp(-spreadAngle / 2, spreadAngle / 2, (sporesPerWave == 1 ? 0.5f : i / (float)(sporesPerWave - 1)));
            Vector3 right = Vector3.Cross(Vector3.up, baseDir);
            Vector3 spreadDir = (baseDir + right * Mathf.Tan(offset * Mathf.Deg2Rad)).normalized;

            // Rotación de la espora hacia spreadDir
            Quaternion rotation = Quaternion.LookRotation(spreadDir);

            GameObject spore = Instantiate(sporePrefab, sporeSpawnPoint.position, rotation);
            spore.GetComponent<SporeProjectile>().Init(sporeTargetPoint);
        }
    }
}
