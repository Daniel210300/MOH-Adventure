using UnityEngine;

public class SporeProjectile : MonoBehaviour
{
    public float speed = 7f;
    public float damage = 25f;

    private Transform target;

    public void Init(Transform targetTransform)
    {
        target = targetTransform;
    }

    void Update()
    {
        if (target == null) return;

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
        transform.forward = dir;

        // Destruir si llega cerca
        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            // Hacer daño
            var playerEnergy = target.GetComponent<PlayerEnergy>();
            if (playerEnergy != null)
                playerEnergy.TakeDamage(damage);

            // Activar animación Hit
            var animator = target.GetComponent<Animator>();
            if (animator != null)
                animator.SetTrigger("Hit");

            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Hacer daño
            var playerEnergy = other.GetComponent<PlayerEnergy>();
            if (playerEnergy != null)
                playerEnergy.TakeDamage(damage);

            // Activar animación Hit
            var animator = other.GetComponent<Animator>();
            if (animator != null)
                animator.SetTrigger("Hit");

            Destroy(gameObject);
        }

        if (other.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }

}
