using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageOverTimeReceiver : MonoBehaviour
{
    private Dictionary<object, Coroutine> activeDoTs = new Dictionary<object, Coroutine>();

    // inicia o actualiza un DoT por origen (por ejemplo la RootHazard instance)
    public void StartDoT(object source, float dps, float interval, ParticleSystem effect = null, AudioClip sound = null, float knockback = 0f)
    {
        if (activeDoTs.ContainsKey(source)) return;

        Coroutine c = StartCoroutine(DoTRoutine(source, dps, interval, effect, sound, knockback));
        activeDoTs.Add(source, c);
    }

    public void StopDoTFrom(object source)
    {
        if (!activeDoTs.ContainsKey(source)) return;
        StopCoroutine(activeDoTs[source]);
        activeDoTs.Remove(source);
    }

    private IEnumerator DoTRoutine(object source, float dps, float interval, ParticleSystem effect, AudioClip sound, float knockback)
    {
        var cc = GetComponent<CharacterController>();
        var playerEnergy = GetComponent<PlayerEnergy>();

        while (true)
        {
            if (playerEnergy != null && dps > 0f)
            {
                playerEnergy.AddEnergy(-dps * interval); // resta dps * intervalo
            }

            // efecto y sonido por tick (opcional)
            if (effect != null)
                Instantiate(effect, transform.position, Quaternion.identity);
            if (sound != null)
                AudioSource.PlayClipAtPoint(sound, transform.position);

            // knockback: empuja si hay CharacterController
            if (knockback != 0f && cc != null)
            {
                Vector3 away = (transform.position - (transform.position)).normalized; // placeholder
                // mejor: empuja en dirección del normal de la raíz; aquí usamos Vector3.up*0 para evitar errores
                // Para un empuje simple hacia atrás del jugador:
                away = transform.forward * -1f;
                cc.Move(away * knockback * interval);
            }

            yield return new WaitForSeconds(interval);
        }
    }
}
