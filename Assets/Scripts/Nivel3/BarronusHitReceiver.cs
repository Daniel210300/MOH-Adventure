using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BarronusHitReceiver : MonoBehaviour
{
    [Header("Stats")]
    public int life = 3;
    private bool isDead = false;

    [Header("Shake On Hit")]
    public float shakeIntensity = 0.08f;
    public float shakeDuration = 0.2f;

    [Header("Death Settings")]
    public float sinkSpeed = 0.5f; 
    public float sinkDepth = -2f; 

    [Header("Components")]
    public Animator animator;
    private Vector3 originalPos;

    [Header("Attack Script")]
    public BarronusAttack attackScript; // referencia al script de ataque

    [Header("Particles")]
    public ParticleSystem[] attackParticles; // partículas de ataque que quieras detener al morir

    // Cola para procesar hits sin romper memoria
    private Queue<GameObject> pendingHits = new Queue<GameObject>();

    void Start()
    {
        originalPos = transform.localPosition;
    }

    // Al entrar en trigger, solo encolamos la bola
    void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("LightOrb"))
        {
            pendingHits.Enqueue(other.gameObject);
        }
    }

    void Update()
    {
        while (pendingHits.Count > 0)
        {
            var orb = pendingHits.Dequeue(); // orb es GameObject
            Destroy(orb);

            life--;
            Debug.Log("Barronus recibió daño. Vida: " + life);

            // Temblor
            StartCoroutine(Shake());

            // Animación de golpe
            if (animator != null)
                animator.SetTrigger("Hit");

            // Destruir bola
            Destroy(orb);

            if (life <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("¡Barronus murió!");

        // Animación de muerte
        if (animator != null)
            animator.SetTrigger("Die");

        // Detener ataque de esporas
        if (attackScript != null)
            attackScript.isDead = true;

        // Detener partículas de ataque
        if (attackParticles != null)
        {
            foreach (var ps in attackParticles)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

        // Hundirse después de la animación
        StartCoroutine(SinkIntoGround());

        // Desactivar collider para que no siga recibiendo hits
        StartCoroutine(DisableColliderDelay());
    }

    // TEMBLOR CUANDO RECIBE HIT
    IEnumerator Shake()
    {
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeIntensity, shakeIntensity);
            float z = Random.Range(-shakeIntensity, shakeIntensity);
            transform.localPosition = originalPos + new Vector3(x, 0, z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }

    // HUNDIRLO AL MORIR
    IEnumerator SinkIntoGround()
    {
        yield return new WaitForSeconds(1f); // tiempo para dejar correr la animación de muerte
        while (transform.position.y > sinkDepth)
        {
            transform.position += Vector3.down * sinkSpeed * Time.deltaTime;
            yield return null;
        }
        // opcional: destruir objeto después
        // Destroy(gameObject);
    }

    IEnumerator DisableColliderDelay()
    {
        yield return new WaitForSeconds(0.8f);
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
    }
}
