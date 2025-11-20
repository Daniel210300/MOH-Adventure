using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public LightCollector collector;
    public Transform target; // Barronus

    void Update()
    {
        var keyboard = Keyboard.current;
        if(keyboard == null) return;

        // Tecla F para lanzar bola de luz
        if(keyboard.fKey.wasPressedThisFrame)
        {
            if(collector.HasOrbs())
            {
                collector.LaunchOrb(target);
            }
        }
    }
}
