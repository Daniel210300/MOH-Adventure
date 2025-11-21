using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class LightChallenge : MonoBehaviour
{
    [Header("Reto de Fragmentos")]
    public int fragmentsRequired = 5;
    public float timeLimit = 30f;

    [HideInInspector] public int fragmentsCollected = 0;
    private float timer = 0f;
    private bool challengeActive = false;

    [Header("Referencias")]
    public UIManagerLumina uiManager;
    public Animator doorAnimator;
    public GameObject[] fragmentObjects;

    [Header("UI")]
    public GameObject restartButton;

    private void Start()
    {
        if (restartButton != null)
            restartButton.SetActive(false);
    }

    private void Update()
    {
        if (challengeActive)
        {
            timer -= Time.deltaTime;

            if (uiManager != null)
                uiManager.UpdateTimer(timer);

            if (fragmentsCollected >= fragmentsRequired)
            {
                ChallengeCompleted();
            }

            if (timer <= 0f)
            {
                ChallengeFailed();
            }
        }
        else
        {
            if (fragmentsCollected >= fragmentsRequired)
            {
                if (uiManager != null)
                    uiManager.UpdateMessage("¡Reto completado! Presiona 'O' para salir.");

                if (Keyboard.current.oKey.wasPressedThisFrame)
                {
                    ExitChallenge();
                }
            }
        }
    }

    public void StartChallenge()
    {
        fragmentsCollected = 0;
        timer = timeLimit;
        challengeActive = true;

        if (uiManager != null)
            uiManager.UpdateMessage("¡Recoge todos los fragmentos de luz!");

        // Activar fragmentos de manera segura
        StartCoroutine(EnableFragmentsSmooth());

        if (restartButton != null)
            restartButton.SetActive(false);
    }

    public void CollectFragment(GameObject fragment)
    {
        fragmentsCollected++;

        if (fragment != null)
            StartCoroutine(DisableNextFrame(fragment));

        if (uiManager != null)
            uiManager.UpdateMessage("Fragmentos recolectados: " + fragmentsCollected + "/" + fragmentsRequired);

        if (fragmentsCollected >= fragmentsRequired)
        {
            OpenDoor();
            ChallengeCompleted();
        }
    }

    private void ChallengeCompleted()
    {
        challengeActive = false;

        if (uiManager != null)
            uiManager.UpdateMessage("¡Reto completado!");
    }

    private void ChallengeFailed()
    {
        challengeActive = false;

        if (uiManager != null)
            uiManager.UpdateMessage("Se acabó el tiempo. Presiona 'Reiniciar'.");

        if (restartButton != null)
            restartButton.SetActive(true);
    }

    public void ResetChallenge()
    {
        fragmentsCollected = 0;
        timer = timeLimit;
        challengeActive = true;

        if (uiManager != null)
        {
            uiManager.UpdateTimer(timeLimit);
            uiManager.ClearMessage();
        }

        StartCoroutine(EnableFragmentsSmooth());

        if (restartButton != null)
            restartButton.SetActive(false);
    }

    private void ExitChallenge()
    {
        Debug.Log("Saliendo del reto de fragmentos...");

    if (uiManager != null)
        uiManager.ClearMessage();
    }

    private void OpenDoor()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("isOpen");
            Debug.Log("¡Se abrió la puerta de ramas!");
        }
        else
        {
            Debug.LogWarning("Animator de DoorRamas2 no asignado!");
        }
    }

    // ========== FIX CRÍTICO ==========
    private IEnumerator DisableNextFrame(GameObject obj)
    {
        yield return null; // Evita leak al desactivar renderers/colliders
        obj.SetActive(false);
    }

    private IEnumerator EnableFragmentsSmooth()
    {
        yield return null; // Evita spike de activaciones simultáneas

        foreach (var f in fragmentObjects)
        {
            f?.SetActive(true);
        }
    }
}
