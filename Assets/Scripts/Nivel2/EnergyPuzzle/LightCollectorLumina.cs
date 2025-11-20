using UnityEngine;

public class LuminaCollectorLumina : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Detecta si choca con un fragmento
        if (other.CompareTag("LightFragment"))
        {
            // Buscar el LightChallenge en la escena
            LightChallenge challenge = FindFirstObjectByType<LightChallenge>();
            if (challenge != null)
            {
                challenge.CollectFragment(other.gameObject); // ✅ PASAR EL FRAGMENTO
            }

            // Destruir el fragmento
            other.gameObject.SetActive(false);

            Debug.Log("Fragmento recolectado por Lumina");
        }
    }
}
