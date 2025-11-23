using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup gameOverGroup;
    public TextMeshProUGUI gameOverText;
    public Button retryButton;
    public Button quitButton;

    [Header("Fade Settings")]
    public float fadeDuration = 0.6f;

    private bool isGameOver = false;

    void Start()
    {
        // Inicia invisible
        gameOverGroup.alpha = 0;
        gameOverGroup.interactable = false;
        gameOverGroup.blocksRaycasts = false;

        // Botones
        retryButton.onClick.AddListener(OnRetry);
        quitButton.onClick.AddListener(QuitToMenu);
    }

    public void ShowGameOver(string message)
    {
        if (isGameOver) return;

        isGameOver = true;
        gameOverText.text = message;

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime; // usar unscaled para ignorar Time.timeScale
            gameOverGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        gameOverGroup.interactable = true;
        gameOverGroup.blocksRaycasts = true;
    }

    // --------------------------
    //          BOTONES
    // --------------------------

    private void OnRetry()
    {
        // ⚡ Indicamos que se está haciendo Retry para saltar el intro
        LevelIntro.SetRetry();

        Time.timeScale = 1f; // por si estaba pausado
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // cambia si tu menu tiene otro nombre
    }
}
