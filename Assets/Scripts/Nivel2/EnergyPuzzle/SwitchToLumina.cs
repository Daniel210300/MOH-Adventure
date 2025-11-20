using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class SwitchToLumina : MonoBehaviour
{
    [Header("Characters")]
    public GameObject hero;
    public GameObject lumina;

    [Header("Cinemachine Cameras")]
    public CinemachineCamera heroCamera;
    public CinemachineCamera topDownCamera;

    [Header("Components to Disable / Enable")]
    public MonoBehaviour heroController;            // Script de Moh
    public MonoBehaviour luminaFollower;            // Si Lumina sigue a Moh antes
    public PlayerControllerLumina luminaController; // Control de Lumina

    private CharacterController luminaCC;
    private bool switched = false;
    public Canvas luminaCanvas;

    private void Awake()
    {
        luminaCC = lumina.GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!switched && other.gameObject == hero)
        {
            SwitchControl();
        }
    }

    void SwitchControl()
{
    switched = true;

    // Desactivar Moh
    heroController.enabled = false;

    // Lumina deja de seguir a Moh (si aplica)
    luminaFollower.enabled = false;

    // Activar control de Lumina
    luminaController.enabled = true;
    luminaCC.enabled = true;

    Debug.Log("Control cambiado a Lumina");

    // Cambiar cámaras
    heroCamera.gameObject.SetActive(false);
    topDownCamera.gameObject.SetActive(true);

    // 🔹 Activar HUD de Lumina
    if (luminaCanvas != null)
    {
        luminaCanvas.gameObject.SetActive(true);
        Debug.Log("LuminaCanvas activado correctamente");
        Debug.Log("Canvas Render Mode: " + luminaCanvas.renderMode);

        if (luminaCanvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Debug.Log("Canvas tiene asignada la cámara: " + luminaCanvas.worldCamera);
        }
    }
    else
    {
        Debug.LogWarning("No se ha asignado LuminaCanvas en el inspector!");
    }

    // 🔹 Iniciar el reto de fragmentos al aparecer Lumina
    LightChallenge challenge = LightChallenge.FindFirstObjectByType<LightChallenge>();
    if (challenge != null)
    {
        challenge.StartChallenge();
        Debug.Log("Reto de fragmentos iniciado");
    }
}


    private void Update()
    {
        if (!switched) return;

        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            ExitArea();
        }
    }

    void ExitArea()
    {
        switched = false;

        // Volver a activar Moh
        heroController.enabled = true;

        // Lumina vuelve a seguir (si aplica)
        luminaFollower.enabled = true;

        // Desactivar control de Lumina
        luminaController.enabled = false;

        // Cámaras
        heroCamera.gameObject.SetActive(true);
        topDownCamera.gameObject.SetActive(false);

        Debug.Log("Regresaste al Moh");
        // Desactivar Canvas de Lumina
    if (luminaCanvas != null)
        luminaCanvas.gameObject.SetActive(false);
    }
}
