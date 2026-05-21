using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ControladorFinal : MonoBehaviour
{
    [Header("Configuración del Director")]
    public PlayableDirector director;

    [Header("Elementos de UI")]
    public GameObject textoHistoria;
    public GameObject textoCreditos;

    [Header("Ajustes de Escena")]
    public string nombreMenu = "Menu";

    void Start()
    {
        // BLOQUEO ANTIPAUSA: En cuanto el objeto final aparece, 
        // avisamos al script PauseMenu de que no deje abrir el menú.
        PauseMenu.finalAlcanzado = true;
    }

    void OnEnable()
    {
        if (director != null)
            director.stopped += AlTerminar;
    }

    void OnDisable()
    {
        if (director != null)
            director.stopped -= AlTerminar;
    }

    void AlTerminar(PlayableDirector a)
    {
        // SEGURIDAD: Resetear tiempo y audio por si venimos de una pausa
        Time.timeScale = 1f;
        AudioListener.pause = false;

        // SEGURIDAD: Liberar el cursor para poder usar el menú
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Cargar la escena del menú
        if (!string.IsNullOrEmpty(nombreMenu))
        {
            SceneManager.LoadScene(nombreMenu);
        }
        else
        {
            Debug.LogError("¡Nicolás, te has olvidado de poner el nombre de la escena 'Menu' en el Inspector!");
        }
    }

    // --- FUNCIONES PARA EL TIMELINE ---
    public void MostrarHistoria() { if (textoHistoria != null) textoHistoria.SetActive(true); }
    public void OcultarHistoria() { if (textoHistoria != null) textoHistoria.SetActive(false); }
    public void MostrarCreditos() { if (textoCreditos != null) textoCreditos.SetActive(true); }
}