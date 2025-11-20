using UnityEngine;

public class BarronusController : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public Animator anim;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        // Activar animación de Hit
        if (anim) anim.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        if (anim) anim.SetTrigger("Die");
        // Aquí desactivas ataques, IA, colisión, etc.
    }
}
