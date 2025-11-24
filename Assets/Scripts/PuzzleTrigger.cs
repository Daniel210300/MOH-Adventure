using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    public HUDObjectives hudObjectives;  // arrastra el HUD
    [TextArea] public string objectiveText; // objetivo específico del trigger

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            hudObjectives.SetObjectiveText(objectiveText); // cambia al texto configurado
        }
    }
}
