using UnityEngine;

public class Crystal : MonoBehaviour
{
    public int crystalValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectCrystal();
        }
    }

    private void CollectCrystal()
    {
        // Verificar que el CrystalCounter existe antes de llamar al método
        if (CrystalCounter.Instance != null)
        {
            CrystalCounter.Instance.AddCrystal();
        }
        else
        {
            Debug.LogError("CrystalCounter.Instance es null. Asegúrate de que existe en la escena.");
        }

        // Efectos opcionales al recolectar
        // AudioManager.Instance.PlaySound("CrystalCollect");
        // Instantiate(collectEffect, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }
}