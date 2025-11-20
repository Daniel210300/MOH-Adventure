using UnityEngine;

public class CrystalCounter : MonoBehaviour
{
    public int crystals;
    public TablaCristales boardText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crystal"))
        {
            crystals++;

            if (boardText != null)
                boardText.UpdateText(crystals);

            Destroy(other.gameObject);
       }
    }
}
