using UnityEngine;

public class LightCollector : MonoBehaviour
{
    public Transform orbSpawnPoint;        // Empty donde salen las orbs
    public int maxOrbs = 2;
    private int currentOrbs = 0;
    public GameObject lightOrbPrefab;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LightOrb"))
        {
            Destroy(other.gameObject);
            currentOrbs++;
            Debug.Log("Bola recolectada! Total: " + currentOrbs);
        }
    }

    public bool HasOrbs() => currentOrbs > 0;

    public void LaunchOrb(Transform target)
    {
        if (currentOrbs <= 0 || target == null) return;

        Vector3 spawnPos = orbSpawnPoint != null ? orbSpawnPoint.position : transform.position;

        GameObject orb = Instantiate(lightOrbPrefab, spawnPos, Quaternion.identity);

        LightOrb lo = orb.GetComponent<LightOrb>();
        lo.Init(target);

        currentOrbs--;
        Debug.Log("¡Bola lanzada hacia " + target.name + "!");
    }
}
