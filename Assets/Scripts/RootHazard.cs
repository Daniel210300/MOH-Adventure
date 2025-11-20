using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RootHazard : MonoBehaviour
{
    [Header("Daño")]
    public float damageAmount = 15f; // cuánto daño causa al tocar
    public float knockbackForce = 5f;

    [Header("Efectos")]
    public ParticleSystem hitEffect;
    public AudioClip hitSound;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true; // asegúrate de marcarlo como trigger
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detectado con: " + other.name);
        if (!other.CompareTag("Player")) return;
        Debug.Log("Haciendo daño a Moh");

        // Aplica daño al jugador
        PlayerEnergy playerEnergy = other.GetComponent<PlayerEnergy>();
        if (playerEnergy != null)
            playerEnergy.AddEnergy(-damageAmount); // resta energía

        // Activa animación de golpe
        Animator anim = other.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Hit"); // debe coincidir con el nombre del trigger
        }

        // Efectos visuales y sonido
        if (hitEffect != null)
            Instantiate(hitEffect, other.transform.position + Vector3.up * 0.5f, Quaternion.identity);

        if (hitSound != null)
            AudioSource.PlayClipAtPoint(hitSound, transform.position);

        // Knockback (retroceso leve)
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null && knockbackForce > 0f)
        {
            Vector3 dir = (other.transform.position - transform.position).normalized;
            dir.y = 0.2f; // un pequeño empujón hacia arriba
            cc.Move(dir * knockbackForce * Time.deltaTime);
        }
    }
}
