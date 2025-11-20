using UnityEngine;

public class LightOrb : MonoBehaviour
{
    public float speed = 10f;
    public Transform moveTarget;
    public float stopDistance = 0.3f; // más pequeño para impactar mejor

    void Update()
    {
        if(moveTarget == null) return;

        // Dirección hacia el HitPoint
        Vector3 dir = (moveTarget.position - transform.position).normalized;

        // Distancia al objetivo
        float dist = Vector3.Distance(transform.position, moveTarget.position);

        // Mientras no llega al punto exacto
        if (dist > stopDistance)
        {
            transform.position += dir * speed * Time.deltaTime;
        }
        else
        {
            // Impacto del orb
            Impact();
        }
    }

    void Impact()
    {
        // Aquí explota, partículas, sonido, etc.
        Destroy(gameObject);
    }
}
