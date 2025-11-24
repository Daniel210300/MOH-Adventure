using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelCompleteManager : MonoBehaviour
{
    public static LevelCompleteManager Instance;

    [Header("UI")]
    public CanvasGroup levelCompleteGroup;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    private void Awake()
    {
        Instance = this;
        levelCompleteGroup.alpha = 0;
        levelCompleteGroup.interactable = false;
        levelCompleteGroup.blocksRaycasts = false;
    }

    public void ShowLevelComplete()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            levelCompleteGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        levelCompleteGroup.interactable = true;
        levelCompleteGroup.blocksRaycasts = true;

        Time.timeScale = 0f; // pausa el juego
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;

        string currentScene = SceneManager.GetActiveScene().name;

        switch (currentScene)
        {
            case "Nivel1":
                SceneManager.LoadScene("Nivel2");
                break;
            case "Nivel2":
                SceneManager.LoadScene("Nivel3");
                break;
            case "Nivel3":
            default:
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
