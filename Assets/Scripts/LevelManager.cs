using UnityEngine;
using UnityEngine.SceneManagement; // para obtener el nombre de la escena

public class LevelManager : MonoBehaviour
{
    public HUDObjectives hudObjectives;

    void Start()
    {
        if (hudObjectives != null)
        {
            string currentScene = SceneManager.GetActiveScene().name;

            if (currentScene == "Nivel2")
            {
                hudObjectives.SetObjectiveText("Salir de la cueva");
            }
            else if (currentScene == "Nivel3")
            {
                hudObjectives.SetObjectiveText("Buscar a Barronus y derrotarlo");
            }
        }
    }

    // Método público para actualizar objetivo en cualquier momento
    public void UpdateObjective(string newObjective)
    {
        if (hudObjectives != null)
            hudObjectives.SetObjectiveText(newObjective);
    }
}
