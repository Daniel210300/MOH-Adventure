using UnityEngine;
using System.Collections;

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

        if (isRetry)
        {
            // Si es Retry, no mostrar intro
            if (player != null)
                player.canMove = true;

            // Reset para la próxima vez
            isRetry = false;
            return;
        }

        // Bloquear movimiento del jugador al inicio
        if (player != null)
            player.canMove = false;

        if (subtitleSystem != null)
            StartCoroutine(ShowSubtitlesSequence());
        else
            Debug.LogError("No se encontró LuminaSubtitleSystem en la escena!");
    }

    IEnumerator ShowSubtitlesSequence()
    {
        float dur;

        yield return new WaitForSeconds(0.5f);

        dur = 6.5f;
        subtitleSystem.LuminaDice(
            "Moh... puedes oirme. Eres fuerte, y estoy aqui para guiarte.",
            dur
        );
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 3f;
        subtitleSystem.LuminaDice(
            "Estas en una Zona Segura, el unico lugar donde la oscuridad no puede tocarte.",
            dur
        );
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 2.5f;
        subtitleSystem.LuminaDice(
            "Descansa aqui, pero no por mucho tiempo.",
            dur
        );
        yield return new WaitForSeconds(dur + 0.5f);

        // Subtítulos tutoriales
        dur = 2f;
        subtitleSystem.LuminaDice("Mira adelante.", dur);
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 4f;
        subtitleSystem.LuminaDice("Esos champiñones han absorbido un poco de energia luminica.", dur);
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 4f;
        subtitleSystem.LuminaDice("Puedes usarlos para rebotar y alcanzar lugares mas altos.", dur);
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 4.5f;
        subtitleSystem.LuminaDice("Barronus ha robado la luz. Necesitamos recuperar estos fragmentos de luz.", dur);
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 5f;
        subtitleSystem.LuminaDice("Pero cuidado... Si la oscuridad te debilita, toma los fragmentos.", dur);
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 3f;
        subtitleSystem.LuminaDice("Ellos restauraran tu energia.", dur);
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
