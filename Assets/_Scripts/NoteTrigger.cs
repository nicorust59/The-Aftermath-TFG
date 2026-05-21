using UnityEngine;
using UnityEngine.UI; // Si usas Texto normal
using TMPro;          // Si usas TextMeshPro

public class NoteTrigger : MonoBehaviour
{
    [Header("Configuración UI")]
    public GameObject panelNotaUI; // El panel que acabamos de crear (PanelNota)
    public GameObject avisoPantalla; // (Opcional) Texto pequeño tipo "Pulsa E para leer"

    [Header("Sonidos")]
    public AudioSource origenSonido;
    public AudioClip sonidoPapel; // Sonido de arrugar papel al abrir/cerrar

    private bool cercaDeNota = false;
    private bool leyendo = false;

    void Start()
    {
        // Aseguramos que todo empiece oculto
        if (panelNotaUI != null) panelNotaUI.SetActive(false);
        if (avisoPantalla != null) avisoPantalla.SetActive(false);
    }

    void Update()
    {
        // Solo si estamos cerca podemos interactuar
        if (cercaDeNota && Input.GetKeyDown(KeyCode.E))
        {
            if (leyendo)
            {
                CerrarNota();
            }
            else
            {
                AbrirNota();
            }
        }
    }

    void AbrirNota()
    {
        leyendo = true;
        panelNotaUI.SetActive(true);
        if (avisoPantalla != null) avisoPantalla.SetActive(false); // Ocultamos el aviso

        // Pausar el juego y liberar el ratón
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlaySonido();
    }

    void CerrarNota()
    {
        leyendo = false;
        panelNotaUI.SetActive(false);
        if (avisoPantalla != null) avisoPantalla.SetActive(true); // Vuelve el aviso

        // Reanudar juego y bloquear ratón
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlaySonido();
    }

    void PlaySonido()
    {
        if (origenSonido != null && sonidoPapel != null)
        {
            // Usamos PlayOneShot para que suene incluso si el juego se pausa
            origenSonido.PlayOneShot(sonidoPapel);
        }
    }

    // DETECTORES DE ZONA (ENTRAR Y SALIR)
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cercaDeNota = true;
            if (avisoPantalla != null) avisoPantalla.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cercaDeNota = false;
            if (avisoPantalla != null) avisoPantalla.SetActive(false);

            // Si te alejas leyendo, se cierra sola por seguridad
            if (leyendo) CerrarNota();
        }
    }
}