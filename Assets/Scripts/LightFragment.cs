using UnityEngine;

public class LightFragment : MonoBehaviour
{
    public float energyAmount = 20f;
    public GameObject pickupEffect; // referencia al efecto hijo (la luz animada)

    private bool collected = false;

    private void Start()
    {
        // Asegura que el efecto esté apagado al iniciar
        if (pickupEffect != null)
            pickupEffect.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            // 🔹 Suma energía al jugador
            PlayerEnergy playerEnergy = other.GetComponent<PlayerEnergy>();
            if (playerEnergy != null)
            {
                playerEnergy.AddEnergy(energyAmount);

                // 💫 Hace reaccionar a Lumina si está en la escena
                LuminaReact lumina = FindFirstObjectByType<LuminaReact>();
                if (lumina != null)
                    lumina.React();
            }

            // 🔹 Activa el efecto visual solo al recoger
            if (pickupEffect != null)
            {
                pickupEffect.SetActive(true);  
                Animator anim = pickupEffect.GetComponent<Animator>();
                if (anim != null)
                    anim.Play("LightFade", 0, 0f);
            }

            // 🔹 Oculta el resto del fragmento
            foreach (Transform child in transform)
            {
                if (child.gameObject != pickupEffect)
                    child.gameObject.SetActive(false);
            }

            // 🔹 Notificar al reto de tiempo
            LightChallenge challenge = LightChallenge.FindFirstObjectByType<LightChallenge>();
if (challenge != null)
{
    challenge.CollectFragment(this.gameObject); // pasar el fragmento recogido
}



            // 🔹 Desactiva el collider y destruye el objeto tras un retardo
            GetComponent<Collider>().enabled = false;
            gameObject.SetActive(false);
        }
    }
}
