using UnityEngine;

public class LightCollector : MonoBehaviour
{
    public Transform launchPoint;
    public int maxOrbs = 2; // máximo que puede sostener Moh
    private int currentOrbs = 0;

    public GameObject lightOrbPrefab; // prefab de la bola de luz

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("LightOrb"))
        {
            Destroy(other.gameObject);
            currentOrbs++;
            Debug.Log("Bola recolectada! Total: " + currentOrbs);
        }
    }

    public bool HasOrbs()
    {
        return currentOrbs > 0;
    }

    public void LaunchOrb(Transform target)
    {
        if (currentOrbs <= 0) return;

        // Si no asignaste LaunchPoint, usar posición del player como fallback
        Vector3 spawnPos = launchPoint != null 
            ? launchPoint.position 
            : (transform.position + Vector3.up * 1.5f + transform.forward * 1f);

        GameObject orb = Instantiate(lightOrbPrefab, spawnPos, Quaternion.identity);

        LightOrb lo = orb.GetComponent<LightOrb>();
        lo.moveTarget = target;
        lo.speed = 5f;

        currentOrbs--;
        Debug.Log("Bolas disponibles: " + HasOrbs());
        Debug.Log("¡Bola lanzada hacia " + target.name + "!");
    }
}
