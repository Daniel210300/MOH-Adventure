using UnityEngine;

public class BounceMushroom : MonoBehaviour
{
    public float bounceForce = 15f;
    public Animator animator;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Reinicia velocidad vertical antes del rebote
                Vector3 vel = rb.linearVelocity;
                vel.y = 0;
                rb.linearVelocity = vel;

                // Aplica impulso hacia arriba
                rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            }

            if (animator != null)
                animator.SetTrigger("Bounce");
        }
    }
}
