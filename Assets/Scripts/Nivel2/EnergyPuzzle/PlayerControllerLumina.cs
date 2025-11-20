using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerLumina : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 5f;
    private float gravity = -30f;
    private float verticalVelocity = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        float x = 0f;
        float z = 0f;

        if (kb.wKey.isPressed) x = -1f;
        if (kb.sKey.isPressed) x = 1f;
        if (kb.dKey.isPressed) z = 1f;
        if (kb.aKey.isPressed) z = -1f;

        Vector3 move = new Vector3(x, 0f, z).normalized * speed;

        // Gravedad
        if (controller.isGrounded)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);

        // Rotación hacia la dirección de movimiento
        Vector3 flatMove = new Vector3(move.x, 0f, move.z);

        // Protección: solo rotar si hay movimiento y no hay NaN
        if (flatMove.sqrMagnitude > 0.001f &&
            !float.IsNaN(flatMove.x) &&
            !float.IsNaN(flatMove.z))
        {
            Quaternion targetRot = Quaternion.LookRotation(flatMove);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.15f);
        }
    }
}
