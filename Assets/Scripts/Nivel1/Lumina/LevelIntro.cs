using UnityEngine;
using System.Collections;

public class LevelIntro : MonoBehaviour
{
    private LuminaSubtitleSystem subtitleSystem;
    private PlayerController player; // ← Referencia al jugador

    private void Start()
    {
        subtitleSystem = FindFirstObjectByType<LuminaSubtitleSystem>();
        player = FindFirstObjectByType<PlayerController>();

        // Bloquear movimiento al inicio
        if (player != null)
            player.canMove = false;

        if(subtitleSystem != null)
            StartCoroutine(ShowSubtitlesSequence());
        else
            Debug.LogError("No se encontró LuminaSubtitleSystem en la escena!");
    }

    IEnumerator ShowSubtitlesSequence()
    {
        float dur;

        // Primer subtítulo
        dur = 6.5f;
        subtitleSystem.LuminaDice("Moh... puedes oírme. Eres fuerte, y estoy aquí para guiarte.", dur);
        yield return new WaitForSeconds(dur + 0.5f);

        // Segundo subtítulo
        dur = 3f;
        subtitleSystem.LuminaDice("Estás en una Zona Segura, el único lugar donde la oscuridad no puede tocarte.", dur);
        yield return new WaitForSeconds(dur + 0.5f);

        // Tercer subtítulo
        dur = 2.5f;
        subtitleSystem.LuminaDice("Descansa aquí, pero no por mucho tiempo.", dur);
        yield return new WaitForSeconds(dur + 0.5f);


        // --- NUEVOS SUBTÍTULOS ---

        dur = 2f;
        subtitleSystem.LuminaDice("Mira adelante.", dur);
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 4f;
        subtitleSystem.LuminaDice("Esos champiñones han absorbido un poco de energía lumínica.", dur);
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 4f;
        subtitleSystem.LuminaDice("Puedes usarlos para rebotar y alcanzar lugares más altos.", dur);
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 4.5f;
        subtitleSystem.LuminaDice("Barronus ha robado la luz. Necesitamos recuperar estos fragmentos de luz.", dur);
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 5f;
        subtitleSystem.LuminaDice("Pero cuidado... Si la oscuridad te debilita, toma los fragmentos.", dur);
        yield return new WaitForSeconds(dur + 0.5f);

        dur = 3f;
        subtitleSystem.LuminaDice("Ellos restaurarán tu energía.", dur);
        yield return new WaitForSeconds(dur + 0.5f);


        // 🌟 Restaurar movimiento al final
        if (player != null)
            player.canMove = true;
    }
}
