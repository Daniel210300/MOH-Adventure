using UnityEngine;

public class LuminaFloat : MonoBehaviour
{
    public Transform visual;  // referencia al hijo visual (esfera + partículas)
    public float amplitude = 0.2f;
    public float frequency = 2f;

    private Vector3 startPos;

    void Start()
    {
        if (visual != null)
            startPos = visual.localPosition;
    }

    void Update()
    {
        if (visual != null)
        {
            // Movimiento vertical suave sin afectar el seguimiento
            visual.localPosition = startPos + Vector3.up * Mathf.Sin(Time.time * frequency) * amplitude;
        }
    }
}
