using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelIntro : MonoBehaviour
{
    private LuminaSubtitleSystem subtitleSystem;
    private PlayerController player;

    // 🔹 Variable estática para detectar Retry
    private static bool isRetry = false;

    private void Start()
    {
        subtitleSystem = FindFirstObjectByType<LuminaSubtitleSystem>();
        player = FindFirstObjectByType<PlayerController>();

        string currentScene = SceneManager.GetActiveScene().name;

        if (isRetry)
        {
            // Si es Retry, no mostrar intro
            if (player != null)
                player.canMove = true;

            isRetry = false;
            return;
        }

        // Bloquear movimiento del jugador al inicio
        if (player != null)
            player.canMove = false;

        if (subtitleSystem != null)
        {
            // Selección de diálogos según el nivel
            if (currentScene == "Nivel1")
                StartCoroutine(ShowNivel1Intro());
            else if (currentScene == "Nivel2")
                StartCoroutine(ShowNivel2Intro());
            else if (currentScene == "Nivel3")
                StartCoroutine(ShowNivel3Intro());
        }
        else
        {
            Debug.LogError("No se encontró LuminaSubtitleSystem en la escena!");
        }
    }

    // ----------------- INTRO NIVEL 1 -----------------
    IEnumerator ShowNivel1Intro()
    {
        float dur;

        yield return new WaitForSeconds(0.5f);

        dur = 6.5f;
        subtitleSystem.LuminaDice(
            "Moh... ¿puedes oírme? Eres fuerte y estoy aquí para guiarte.",
            dur
        );
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 3f;
        subtitleSystem.LuminaDice(
            "Estás en una Zona Segura, el único lugar donde la oscuridad no puede tocarte.",
            dur
        );
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 2.5f;
        subtitleSystem.LuminaDice(
            "Descansa aquí, pero no por mucho tiempo.",
            dur
        );
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 2f;
        subtitleSystem.LuminaDice("Mira adelante.", dur);
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 4f;
        subtitleSystem.LuminaDice(
            "Esos champiñones han absorbido un poco de energía lumínica.",
            dur
        );
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 4f;
        subtitleSystem.LuminaDice(
            "Puedes usarlos para rebotar y alcanzar lugares más altos.",
            dur
        );
        yield return new WaitForSeconds(dur + 0.5f);

        if (player != null)
            player.canMove = true;
    }

    // ----------------- INTRO NIVEL 2 -----------------
    IEnumerator ShowNivel2Intro()
    {
        float dur;
        yield return new WaitForSeconds(0.5f);

        dur = 5f;
        subtitleSystem.LuminaDice(
            "Estamos dentro, Moh. Esta cueva es el túnel que lleva al corazón del bosque.",
            dur
        );
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 5f;
        subtitleSystem.LuminaDice(
            "Mira a tu derecha, Moh. Veo un patrón de cristales tallado en la roca. No sé qué significa, pero anótalo.",
            dur
        );
        yield return new WaitForSeconds(dur + 0.5f);

        if (player != null)
            player.canMove = true;
    }

    // ----------------- INTRO NIVEL 3 -----------------
    IEnumerator ShowNivel3Intro()
    {
        float dur;
        yield return new WaitForSeconds(0.5f);

        dur = 4f;
        subtitleSystem.LuminaDice(
            "Esta es la parte más oscura del terreno.",
            dur
        );
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 4f;
        subtitleSystem.LuminaDice(
            "Siento una presencia maligna muy cerca. Moh, mantente alerta.",
            dur
        );
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 3.5f;
        subtitleSystem.LuminaDice(
            "¡Es él! ¡Es Barronus! Esta es solo su primera manifestación.",
            dur
        );
        yield return new WaitForSeconds(dur + 0.5f);

        if (player != null)
            player.canMove = true;
    }

    // 🔹 Método que se llama desde GameOverManager cuando se presiona Retry
    public static void SetRetry()
    {
        isRetry = true;
    }
}
