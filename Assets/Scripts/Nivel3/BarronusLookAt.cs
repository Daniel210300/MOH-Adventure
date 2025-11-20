using UnityEngine;

public class BarronusLookAt : MonoBehaviour
{
    public Transform target; // Moh
    public float rotateSpeed = 2f;

    void Update()
    {
        if (target == null) return;

        // Posición objetivo pero solo XZ (misma altura)
        Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);

        // Rotación hacia Moh
        Quaternion lookRotation = Quaternion.LookRotation(targetPos - transform.position);

        // Suavizar rotación
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
    }
}
