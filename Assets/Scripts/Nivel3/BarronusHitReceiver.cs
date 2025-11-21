using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BarronusHitReceiver : MonoBehaviour
{
    [Header("Stats")]
    public int life = 3;
    public int maxLife = 3;
    private bool isDead = false;

    [Header("UI Elements")]
    public GameObject heartContainer;
    public TextMeshProUGUI nameText;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Vector2 heartSize = new Vector2(40, 40);
    private List<Image> heartImages = new List<Image>();

    [Header("Audio Settings")]
    public AudioClip deathSound;
    public AudioSource audioSource;
    public float deathSoundDelay = 1.8f; // Delay antes del sonido de muerte

    [Header("Finish Game")]
    public GameObject gameOverCanvas;

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
    public BarronusAttack attackScript;

    [Header("Particles")]
    public ParticleSystem[] attackParticles;

    private Queue<GameObject> pendingHits = new Queue<GameObject>();

    void Start()
    {
        originalPos = transform.localPosition;
        InitializeHearts();
        
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void InitializeHearts()
    {
        if (heartContainer == null)
        {
            Debug.LogError("No se asignó el contenedor de corazones!");
            return;
        }

        foreach (Transform child in heartContainer.transform)
        {
            Destroy(child.gameObject);
        }
        heartImages.Clear();

        for (int i = 0; i < maxLife; i++)
        {
            GameObject heartObj = new GameObject("Heart_" + i);
            heartObj.transform.SetParent(heartContainer.transform);
            
            Image heartImage = heartObj.AddComponent<Image>();
            heartImage.sprite = fullHeart;
            heartImage.preserveAspect = true;
            
            RectTransform rect = heartImage.GetComponent<RectTransform>();
            rect.sizeDelta = heartSize;
            
            heartImages.Add(heartImage);
        }

        if (nameText != null)
        {
            nameText.text = "Barronus:";
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < life)
            {
                heartImages[i].sprite = fullHeart;
            }
            else
            {
                heartImages[i].sprite = emptyHeart;
            }
        }
    }

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
        while (pendingHits.Count > 0 && !isDead)
        {
            var orb = pendingHits.Dequeue();
            
            life--;
            Debug.Log("Barronus recibió daño. Vida: " + life);

            UpdateHearts();
            StartCoroutine(Shake());

            if (animator != null)
                animator.SetTrigger("Hit");

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

        // Actualizar corazones (todos vacíos)
        UpdateHearts();

        // Reproducir sonido de muerte CON DELAY
        StartCoroutine(PlayDeathSoundWithDelay());

        // Hundirse después de la animación
        StartCoroutine(SinkIntoGround());

        // Desactivar collider para que no siga recibiendo hits
        StartCoroutine(DisableColliderDelay());
    }

    IEnumerator PlayDeathSoundWithDelay()
    {
        // Esperar el delay antes de reproducir el sonido
        yield return new WaitForSeconds(deathSoundDelay);
        
        // Reproducir sonido de muerte
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
            
            // Esperar a que termine el sonido y luego activar canvas
            StartCoroutine(WaitForSoundToFinish());
        }
        else
        {
            // Si no hay sonido, activar el canvas después del delay
            StartCoroutine(ActivateGameOverCanvas(1f));
        }
    }

    IEnumerator WaitForSoundToFinish()
    {
        // Esperar a que termine el sonido de muerte
        yield return new WaitForSeconds(deathSound.length);
        
        // Activar el canvas de game over
        ActivateGameOver();
    }

    IEnumerator ActivateGameOverCanvas(float delay)
    {
        yield return new WaitForSeconds(delay);
        ActivateGameOver();
    }

    void ActivateGameOver()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            Debug.Log("Canvas de Game Over activado");
        }
        else
        {
            Debug.LogWarning("No se asignó el Game Over Canvas");
        }
    }

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

    IEnumerator SinkIntoGround()
    {
        yield return new WaitForSeconds(1f);
        while (transform.position.y > sinkDepth)
        {
            transform.position += Vector3.down * sinkSpeed * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DisableColliderDelay()
    {
        yield return new WaitForSeconds(0.8f);
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
    }
}
