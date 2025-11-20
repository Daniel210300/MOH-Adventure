using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TMP_Text mensaje;

    void Awake()
    {
        instance = this;
    }

    public void MostrarMensaje(string txt)
    {
        mensaje.text = txt;
        mensaje.gameObject.SetActive(true);
    }

    public void OcultarMensaje()
    {
        mensaje.gameObject.SetActive(false);
    }
}
