using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuPro : MonoBehaviour
{
    [Header("Paneles UI")]
    public GameObject panelPrincipal;
    public GameObject panelOpciones;
    public GameObject panelHistoria;
    public GameObject panelControles;

    [Header("Configuración de Partida")]
    public string nombreEscenaJuego = "Area";

    [Header("Configuración Sliders")]
    public Slider sliderVolumen;
    public Slider sliderSensibilidad;

    [Header("AUDIO INTERFAZ")]
    public AudioSource fuenteAudioUI;
    public AudioClip sonidoClick;

    public static float sensibilidadGuardada = 2.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Al empezar, nos aseguramos de que solo el principal esté activo
        IrAlMenuPrincipal();

        if (sliderVolumen != null) sliderVolumen.value = AudioListener.volume;
        if (sliderSensibilidad != null) sliderSensibilidad.value = sensibilidadGuardada;
    }

    // --- SISTEMA DE GESTIÓN DE PANELES (NUEVO Y SEGURO) ---
    private void CerrarTodosLosPaneles()
    {
        if (panelPrincipal != null) panelPrincipal.SetActive(false);
        if (panelOpciones != null) panelOpciones.SetActive(false);
        if (panelHistoria != null) panelHistoria.SetActive(false);
        if (panelControles != null) panelControles.SetActive(false);
    }

    public void HacerClick()
    {
        if (fuenteAudioUI != null && sonidoClick != null)
        {
            fuenteAudioUI.PlayOneShot(sonidoClick);
        }
    }

    // ==========================================
    // NAVEGACIÓN
    // ==========================================

    public void IrAlMenuPrincipal()
    {
        CerrarTodosLosPaneles();
        panelPrincipal.SetActive(true);
    }

    public void BotonJugar()
    {
        HacerClick();
        CerrarTodosLosPaneles();
        panelHistoria.SetActive(true);
    }

    public void BotonOpciones()
    {
        HacerClick();
        CerrarTodosLosPaneles();
        panelOpciones.SetActive(true);
    }

    public void BotonAyuda() // Desde Menú Principal a Controles
    {
        HacerClick();
        CerrarTodosLosPaneles();
        panelControles.SetActive(true);
    }

    public void BotonVerControles() // Desde Historia a Controles
    {
        HacerClick();
        CerrarTodosLosPaneles();
        panelControles.SetActive(true);
    }

    public void BotonVolverAtras() // ESTA FUNCIÓN SIRVE PARA TODOS LOS BOTONES DE "ATRÁS"
    {
        HacerClick();
        IrAlMenuPrincipal();
    }

    public void BotonVolverAHistoria() // Solo si quieres volver específicamente a historia
    {
        HacerClick();
        CerrarTodosLosPaneles();
        panelHistoria.SetActive(true);
    }

    public void BotonSalir()
    {
        HacerClick();
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    public void BotonEmpezarPartida()
    {
        HacerClick();
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    // ==========================================
    // AJUSTES
    // ==========================================

    public void CambiarVolumen(float valor)
    {
        AudioListener.volume = valor;
    }

    public void CambiarSensibilidad(float valor)
    {
        sensibilidadGuardada = valor;
    }
}