using UnityEngine;
using TMPro;

public class TablaCristales : MonoBehaviour
{
    public TextMeshPro textMesh; 
    public int requiredCrystals = 3;

    void Start()
    {
        UpdateText(0); // Empieza mostrando 0
    }

    public void UpdateText(int currentCrystals)
    {
        textMesh.text = "Cristales: " + currentCrystals + " / " + requiredCrystals;
    }

}
