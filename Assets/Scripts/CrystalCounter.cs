using UnityEngine;

public class CrystalCounter : MonoBehaviour
{
    public static CrystalCounter Instance;

    public int crystals = 0;
    public TablaCristales tabla;

    private void Awake()
    {
        Instance = this;
    }

    public void AddCrystal()
    {
        crystals++;
        tabla.UpdateText(crystals);
    }
}