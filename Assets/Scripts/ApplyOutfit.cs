using UnityEngine;

public class ApplyOutfit : MonoBehaviour
{
    public GameObject ropaNormal;
    public GameObject ropaDark;

    public GameObject sombreroNormal;   // ← NUEVO
    public GameObject sombreroDark;     // ← NUEVO

    void Start()
    {
        int atuendo = PlayerPrefs.GetInt("Atuendo", 0);

        bool normal = (atuendo == 0);
        bool dark = (atuendo == 1);

        // Ropa
        ropaNormal.SetActive(normal);
        ropaDark.SetActive(dark);

        // Sombreros
        sombreroNormal.SetActive(normal);
        sombreroDark.SetActive(dark);
    }
}
