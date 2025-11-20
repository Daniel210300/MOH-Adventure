using UnityEngine;

public class SporeProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float damage = 15f;

    private Transform target;

    // Inicializa la espora apuntando hacia un Transform
    public void Init(Transform targetTransform)
    {
        target = targetTransform;
    }

    void Update()
    {
        if (target == null) return;

        // Movimiento hacia el target
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Colisiona con el Player
        if (other.CompareTag("Player"))
        {
            if (PlayerEnergy.Instance != null)
                PlayerEnergy.Instance.TakeDamage(damage);

            Destroy(gameObject);
        }

        // Colisiona con el entorno
        if (other.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
}
