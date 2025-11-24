using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public LightCollector collector;        // Script de Moh
    // public Transform barronusTargetEmpty; // Ya no se necesita si solo usamos triggers

    void Update()
    {
        // Aquí puedes poner otros ataques de Moh que no dependan de triggers
        // Por ejemplo:
        /*
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Ejecutar otro ataque
        }
        */

        // Si quisieras dejar que desde cualquier lugar Moh lance orbs:
        /*
        if (Keyboard.current.fKey.wasPressedThisFrame && collector.HasOrbs())
        {
            collector.LaunchOrb(barronusTargetEmpty);
        }
        */
        // Pero esta parte la dejamos comentada si quieres que solo se lance desde triggers
    }
}
