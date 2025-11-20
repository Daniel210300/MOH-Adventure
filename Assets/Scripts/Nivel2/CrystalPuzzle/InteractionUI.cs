using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI instance;

    public TMP_Text interactionText;

    void Awake()
    {
        instance = this;
    }

    public void Show(string message)
    {
        interactionText.text = message;
        interactionText.gameObject.SetActive(true);
    }

    public void Hide()
    {
        interactionText.gameObject.SetActive(false);
    }
}
