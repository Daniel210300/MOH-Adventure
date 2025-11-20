using UnityEngine;

public class Crystal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CrystalCounter.Instance.AddCrystal();
            Destroy(gameObject);
        }
    }
}