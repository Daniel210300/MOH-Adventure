using UnityEngine;
using TMPro;

public class UIManagerLumina : MonoBehaviour
{
    public TMP_Text timerText;         // Texto del tiempo
    public TMP_Text luminaMessageText; // Texto de instrucciones

    public void UpdateTimer(float seconds)
    {
        if (timerText != null)
            timerText.text = "Tiempo: " + Mathf.Ceil(seconds).ToString() + "s";
    }

    public void UpdateMessage(string message)
    {
        if (luminaMessageText != null)
            luminaMessageText.text = message;
    }

    public void ClearMessage()
    {
        if (luminaMessageText != null)
            luminaMessageText.text = "";
    }
}
