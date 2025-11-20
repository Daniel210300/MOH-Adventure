using UnityEngine;

public class BarronusAttack : MonoBehaviour
{
    public GameObject sporePrefab;
    public Transform sporeSpawnPoint;
    public Transform playerTarget;

    public float attackInterval = 6f;
    private float timer = 0f;

    public int sporesPerWave = 3;
    public float spreadAngle = 30f;

    [HideInInspector] public bool isDead = false;

    void Update()
    {
        if (playerTarget == null || isDead) return;

        timer += Time.deltaTime;
        if (timer >= attackInterval)
        {
            timer = 0f;
            LaunchSporeWave();
        }
    }

    void LaunchSporeWave()
    {
        for (int i = 0; i < sporesPerWave; i++)
        {
            float angleOffset = Mathf.Lerp(-spreadAngle / 2, spreadAngle / 2, i / (float)(sporesPerWave - 1));
            Vector3 spawnDir = (playerTarget.position - sporeSpawnPoint.position).normalized;
            spawnDir = Quaternion.Euler(0, angleOffset, 0) * spawnDir;

            GameObject spore = Instantiate(sporePrefab, sporeSpawnPoint.position, Quaternion.identity);
            spore.GetComponent<SporeProjectile>().Init(playerTarget); // <-- Aquí pasa el Transform
        }
    }
}
