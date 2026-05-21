using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class GestorCinematicaFinal : MonoBehaviour
{
    [Header("Conexiones")]
    public PlayableDirector directorFinal; // Arrastra tu Director_Final aquí
    public GameObject panelVictoriaUI;     // Arrastra tu PanelVictoria aquí

    public float duracionCinematica = 5.0f; // Lo que dura tu plano de la cámara

    void Start()
    {
        // Por si acaso, nos aseguramos de que el panel de victoria empiece apagado
        if (panelVictoriaUI != null) panelVictoriaUI.SetActive(false);
    }

    // 🔥 TU SCRIPT DE INTERACCIÓN TIENE QUE LLAMAR A ESTA FUNCIÓN 🔥
    public void ActivarFinal()
    {
        StartCoroutine(SecuenciaHuida());
    }

    IEnumerator SecuenciaHuida()
    {
        Debug.Log("🚤 ¡Arrancando motor! Iniciando cinemática...");

        // 1. Le damos al Play a la película (esto apagará al jugador y moverá la cámara)
        if (directorFinal != null)
        {
            directorFinal.Play();
        }

        // 2. Esperamos los segundos que dure la cinemática
        yield return new WaitForSeconds(duracionCinematica);

        // 3. ¡PUM! Pantalla de victoria y pausamos el juego
        if (panelVictoriaUI != null)
        {
            panelVictoriaUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}