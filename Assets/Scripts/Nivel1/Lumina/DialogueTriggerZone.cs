using UnityEngine;

public class DialogueTriggerZone : MonoBehaviour
{
    [Header("ID único de la zona")]
    public string zoneID;

    [Header("Diálogo que Lumina dirá al entrar")]
    [TextArea]
    public string dialogo;

    public float duration = 4f;

    private bool alreadyTriggered = false;

    private LuminaSubtitleSystem subtitleSystem;

    private void Start()
    {
        subtitleSystem = FindFirstObjectByType<LuminaSubtitleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyTriggered) return;
        if (!other.CompareTag("Player")) return;
        if (subtitleSystem == null) return;

        subtitleSystem.LuminaDice(dialogo, duration);
        alreadyTriggered = true;
    }
}
