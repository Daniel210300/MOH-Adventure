using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class LuminaSubtitleSystem : MonoBehaviour
{
    public CanvasGroup group;
    public Image iconLumina; 
    public TextMeshProUGUI subtitleText;

    public float fadeDuration = 0.4f;

    private void Start()
    {
        group.alpha = 0;  
        subtitleText.text = ""; 
    }

    public void LuminaDice(string frase, float tiempo)
    {
        StopAllCoroutines();
        StartCoroutine(SubtitleRoutine(frase, tiempo));
    }

    IEnumerator SubtitleRoutine(string frase, float tiempo)
    {
        subtitleText.text = frase;

        // fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            group.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        group.alpha = 1;

        // mantener visible
        yield return new WaitForSeconds(tiempo);

        // fade out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            group.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        group.alpha = 0;
    }
}
