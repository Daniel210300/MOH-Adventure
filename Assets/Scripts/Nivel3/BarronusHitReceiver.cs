using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BarronusHitReceiver : MonoBehaviour
{
    [Header("Stats")]
    public int life = 3;
    public int maxLife = 3;
    public bool isDead = false; // público para VictoryZone

    [Header("UI Elements")]
    public GameObject heartContainer;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Vector2 heartSize = new Vector2(35, 35);
    private List<UnityEngine.UI.Image> heartImages = new List<UnityEngine.UI.Image>();

    [Header("Audio")]
    public AudioClip deathSound;
    public AudioSource audioSource;
    public float deathSoundDelay = 1.8f;

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
        if (heartContainer == null) return;

        foreach (Transform child in heartContainer.transform)
            Destroy(child.gameObject);

        heartImages.Clear();

        for (int i = 0; i < maxLife; i++)
        {
            GameObject heartObj = new GameObject("Heart_" + i);
            heartObj.transform.SetParent(heartContainer.transform);

            var heartImage = heartObj.AddComponent<UnityEngine.UI.Image>();
            heartImage.sprite = fullHeart;
            heartImage.preserveAspect = true;

            var rect = heartImage.GetComponent<RectTransform>();
            rect.sizeDelta = heartSize;

            heartImages.Add(heartImage);
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < heartImages.Count; i++)
            heartImages[i].sprite = (i < life) ? fullHeart : emptyHeart;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("LightOrb"))
            pendingHits.Enqueue(other.gameObject);
    }

    void Update()
    {
        while (pendingHits.Count > 0 && !isDead)
        {
            var orb = pendingHits.Dequeue();

            life--;
            UpdateHearts();
            StartCoroutine(Shake());

            if (animator != null)
                animator.SetTrigger("Hit");

            Destroy(orb);

            if (life <= 0)
                Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        if (animator != null)
            animator.SetTrigger("Die");

        if (attackScript != null)
            attackScript.isDead = true;

        if (attackParticles != null)
        {
            foreach (var ps in attackParticles)
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        UpdateHearts();

        // ---------------------------
        // Lumina dice algo cuando Barronus muere
        StartCoroutine(LuminaHabla());
        // ---------------------------

        StartCoroutine(PlayDeathSoundWithDelay());
        StartCoroutine(SinkIntoGround());
        StartCoroutine(DisableColliderDelay());
    }

    IEnumerator LuminaHabla()
    {
        float dur = 8f; // duración del subtítulo
        LuminaSubtitleSystem subtitleSystem = Object.FindFirstObjectByType<LuminaSubtitleSystem>();
        if (subtitleSystem != null)
        {
            subtitleSystem.LuminaDice(
                "¡Sí, así se hace! ¡Con eso es suficiente, Moh! Lo hemos forzado a retroceder. Pero no te confíes, volverá.",
                dur
            );
            yield return new WaitForSeconds(dur + 0.5f);
        }
    }

    IEnumerator PlayDeathSoundWithDelay()
    {
        yield return new WaitForSeconds(deathSoundDelay);

        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
            yield return new WaitForSeconds(deathSound.length);
        }

        // Llama al LevelCompleteManager para terminar el nivel
        var levelComplete = Object.FindFirstObjectByType<LevelCompleteManager>();
        if (levelComplete != null)
            levelComplete.ShowLevelComplete();
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
        var col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
    }

    IEnumerator Shake()
    {
        float shakeDuration = 0.2f;
        float shakeIntensity = 0.08f;
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

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        life -= Mathf.RoundToInt(amount);
        Debug.Log("Barronus recibió " + amount + " de daño. Vida actual: " + life);

        UpdateHearts();
        StartCoroutine(Shake());

        if (animator != null)
            animator.SetTrigger("Hit");

        if (life <= 0)
            Die();
    }
}
