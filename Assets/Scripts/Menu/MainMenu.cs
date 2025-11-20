using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void Vestuario()
    {
        SceneManager.LoadScene("WardrobeMenu");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
