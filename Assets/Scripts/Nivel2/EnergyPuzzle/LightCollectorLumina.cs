using UnityEngine;

public class LuminaCollectorLumina : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LightFragment"))
        {
            LightChallenge challenge = FindFirstObjectByType<LightChallenge>();
            challenge?.CollectFragment(other.gameObject);

            StartCoroutine(SafeDisable(other.gameObject));

            Debug.Log("Fragmento recolectado por Lumina");
        }
    }

    private System.Collections.IEnumerator SafeDisable(GameObject obj)
    {
        yield return null; // espera un frame
        obj.SetActive(false);
    }
}
