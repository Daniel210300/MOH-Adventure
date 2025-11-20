using UnityEngine;
using UnityEngine.InputSystem; // 🔹 necesario para Keyboard.current

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
    public GameObject restartButton; // 🟢 Asigna aquí tu botón de reiniciar en el Inspector

    private void Start()
    {
        if (restartButton != null)
            restartButton.SetActive(false); // ocultamos al inicio
    }

    private void Update()
    {
        // Solo contamos tiempo si el reto está activo
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
            // Si el reto ya terminó correctamente, esperar tecla O
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

        if (fragmentObjects != null)
            foreach (var f in fragmentObjects)
                if (f != null)
                    f.SetActive(true);

        if (restartButton != null)
            restartButton.SetActive(false); // ocultamos el botón al iniciar
    }

    public void CollectFragment(GameObject fragment)
    {
        fragmentsCollected++;

        if (fragment != null)
            fragment.SetActive(false);

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
            restartButton.SetActive(true); // mostrar botón al fallar
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

        if (fragmentObjects != null)
            foreach (var f in fragmentObjects)
                if (f != null)
                    f.SetActive(true);

        if (restartButton != null)
            restartButton.SetActive(false); // ocultamos de nuevo
    }

    // 🔹 Nuevo método para salir del reto con O
    private void ExitChallenge()
    {
        Debug.Log("Saliendo del reto de fragmentos...");

        if (uiManager != null)
            uiManager.ClearMessage();

        // Opcional: reiniciar fragmentos para que vuelvan a aparecer
        if (fragmentObjects != null)
            foreach (var f in fragmentObjects)
                if (f != null)
                    f.SetActive(true);

        // Si quieres, aquí podrías devolver el control a Moh, cámaras, etc.
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
}
