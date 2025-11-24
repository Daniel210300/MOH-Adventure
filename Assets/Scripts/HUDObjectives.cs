using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class HUDObjectives : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;
    public float fadeDuration = 1f;
    public float displayTime = 3f;

    public void SetObjectiveText(string text)
    {
        StopAllCoroutines(); // evita solapamientos
        StartCoroutine(FadeObjectiveText(text));
    }

    private IEnumerator FadeObjectiveText(string text)
    {
        objectiveText.text = text;

        // Fade in
        float elapsed = 0f;
        Color color = objectiveText.color;
        color.a = 0f;
        objectiveText.color = color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / fadeDuration);
            objectiveText.color = color;
            yield return null;
        }

        color.a = 1f;
        objectiveText.color = color;

        // Solo fade out para escenas que no sean Nivel1
        if (SceneManager.GetActiveScene().name != "Nivel1")
        {
            yield return new WaitForSeconds(displayTime);

            // Fade out
            elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                color.a = 1 - Mathf.Clamp01(elapsed / fadeDuration);
                objectiveText.color = color;
                yield return null;
            }

            color.a = 0f;
            objectiveText.color = color;
        }
    }
}
