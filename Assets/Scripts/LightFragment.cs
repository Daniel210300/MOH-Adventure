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
            PlayerEnergy playerEnergy = other.GetComponent<PlayerEnergy>();
            if (playerEnergy == null) return;

            // ⛔ NO permitir recoger si el jugador ya está muerto
            if (playerEnergy.IsDead) return;

            collected = true;

            // Sumar energía
            playerEnergy.AddEnergy(energyAmount);

            // Reacción de Lumina (si existe)
            LuminaReact lumina = FindFirstObjectByType<LuminaReact>();
            if (lumina != null)
                lumina.React();

            // Notificar al puzzle
            LightChallenge challenge = FindFirstObjectByType<LightChallenge>();
            if (challenge != null)
            {
                challenge.CollectFragment(this.gameObject);
            }

            // Desactivar inmediatamente
            GetComponent<Collider>().enabled = false;
            gameObject.SetActive(false);
        }
    }
}
