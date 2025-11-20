using UnityEngine;
using UnityEngine.SceneManagement;

public class WardrobeMenu : MonoBehaviour
{
    public GameObject ropaNormal;
    public GameObject ropaDark;

    public GameObject sombreroNormal;
    public GameObject sombreroDark;

    private void Start()
    {
        int atuendo = PlayerPrefs.GetInt("Atuendo", 0);

        if (atuendo == 0) ActivarNormal();
        else ActivarDark();
    }

    public void ActivarNormal()
    {
        ropaNormal.SetActive(true);
        ropaDark.SetActive(false);

        sombreroNormal.SetActive(true);
        sombreroDark.SetActive(false);

        PlayerPrefs.SetInt("Atuendo", 0);
    }

    public void ActivarDark()
    {
        ropaNormal.SetActive(false);
        ropaDark.SetActive(true);

        sombreroNormal.SetActive(false);
        sombreroDark.SetActive(true);

        PlayerPrefs.SetInt("Atuendo", 1);
    }

    public void Jugar()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void Regresar()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
