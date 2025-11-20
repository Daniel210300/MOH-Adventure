using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerEnergy energy = other.GetComponent<PlayerEnergy>();
            if (energy != null)
                energy.SetInSafeZone(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerEnergy energy = other.GetComponent<PlayerEnergy>();
            energy?.SetInSafeZone(false);
        }
    }
}
