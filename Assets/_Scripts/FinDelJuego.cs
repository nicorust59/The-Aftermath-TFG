using UnityEngine;
using UnityEngine.SceneManagement;

public class FinDelJuego : MonoBehaviour
{
    [Header("Escribe el nombre EXACTO de la escena")]
    public string nombreEscenaMenu = "Menu";

    // OnEnable se ejecuta AUTOMÁTICAMENTE en el mismo milisegundo 
    // en que este objeto se enciende en la pantalla.
    void OnEnable()
    {
        Debug.Log("Panel encendido. Cargando menú en 4 segundos...");

        // Invoke llama a la función que le digamos después de X segundos
        Invoke("CargarMenu", 4f);
    }

    void CargarMenu()
    {
        Debug.Log("¡Saltando al Menú!");
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}