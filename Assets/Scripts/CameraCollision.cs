using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform followTarget;    // El objeto que la cámara sigue (normalmente el Player o CameraRoot)
    public float minDistance = 0.5f;  // Distancia mínima antes de pegarse al jugador
    public float maxDistance = 3f;    // Tu distancia normal de cámara
    public float smooth = 10f;        // Qué tan suave se ajusta

    private Vector3 dollyDir;         // Dirección original de la cámara desde el follow target
    private float distance;           // Distancia actual

    void Start()
    {
        // Dirección inicial y distancia inicial
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    void LateUpdate()
    {
        // Dirección en el espacio del target
        Vector3 desiredPos = followTarget.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        // Si hay una pared entre el target y la cámara...
        if (Physics.Linecast(followTarget.position, desiredPos, out hit))
        {
            // Ajusta la distancia para NO atravesar
            distance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }

        // Movimiento suavizado
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            dollyDir * distance,
            Time.deltaTime * smooth
        );
    }
}
