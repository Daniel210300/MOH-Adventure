using UnityEngine;

public class CrystalCounter : MonoBehaviour
{
    public static CrystalCounter Instance;

    [Header("Cristales del nivel")]
    public int totalCrystals = 3;

    [Header("Referencias")]
    public TablaCristales tabla; // Si ya no usas la tabla, bórrala y elimina esto

    [Header("Cristales actuales")]
    public int crystals = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Solo actualizar la tabla si existe
        if (tabla != null)
            tabla.UpdateText(crystals);
    }

    public void AddCrystal()
    {
        crystals++;

        // Actualizar tabla si existe
        if (tabla != null)
            tabla.UpdateText(crystals);
    }

    public bool AreAllCrystalsCollected()
    {
        return crystals >= totalCrystals;
    }
}
