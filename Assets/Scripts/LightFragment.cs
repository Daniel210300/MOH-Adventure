using UnityEngine;

public class LightFragment : MonoBehaviour
{
    public float energyAmount = 20f;

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            // Sumar energía
            PlayerEnergy playerEnergy = other.GetComponent<PlayerEnergy>();
            if (playerEnergy != null)
            {
                playerEnergy.AddEnergy(energyAmount);

                // Reacción de Lumina si existe (esto es seguro)
                LuminaReact lumina = FindFirstObjectByType<LuminaReact>();
                if (lumina != null)
                    lumina.React();
            }

            // Notificar al puzzle
            LightChallenge challenge = FindFirstObjectByType<LightChallenge>();
            if (challenge != null)
            {
                challenge.CollectFragment(this.gameObject);
            }

            // Desactivar inmediatamente (seguro al no tener efectos)
            GetComponent<Collider>().enabled = false;
            gameObject.SetActive(false);
        }
    }
}
