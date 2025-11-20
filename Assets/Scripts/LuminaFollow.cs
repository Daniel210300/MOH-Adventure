using UnityEngine;

public class LuminaFollow : MonoBehaviour
{
    public Transform target; // Moh
    public Vector3 offset = new Vector3(0, 1.5f, -1f);
    public float smoothSpeed = 5f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        // Posición deseada detrás o cerca de Moh
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);

        // Movimiento suave sin vibraciones
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 1f / smoothSpeed);

        // Opcional: que siempre mire a Moh
        transform.LookAt(target.position + Vector3.up * 1.2f);
    }
}
