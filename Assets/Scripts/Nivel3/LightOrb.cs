using UnityEngine;

public class LightOrb : MonoBehaviour
{
    public float speed = 10f;
    public float stopDistance = 0.3f;
    public float damage = 10f;

    private Transform moveTarget;

    // Inicializar orb
    public void Init(Transform targetTransform, float orbDamage = 10f)
    {
        moveTarget = targetTransform;
        damage = orbDamage;
    }

    void Update()
    {
        if (moveTarget == null) return;

        Vector3 dir = (moveTarget.position - transform.position).normalized;
        float dist = Vector3.Distance(transform.position, moveTarget.position);

        if (dist > stopDistance)
        {
            transform.position += dir * speed * Time.deltaTime;
            transform.forward = dir;
        }
        else
        {
            Impact();
        }
    }

    void Impact()
{
    if (moveTarget == null) return;

    // Busca BarronusHitReceiver en cualquier padre del target
    var barronus = moveTarget.GetComponentInParent<BarronusHitReceiver>();
    if (barronus != null)
    {
        barronus.TakeDamage(damage);
        var animator = barronus.GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("Hit");

            Debug.Log("Orb llegó a: " + moveTarget.name);

        Debug.Log("¡Barronus recibió " + damage + " de daño!");
    }

    Destroy(gameObject);
}

}
