using UnityEngine;
using System.Collections;
using TMPro; // NECESARIO PARA EL TEXTO

public class Teletransportador : MonoBehaviour
{
    [Header("Configuración de Viaje")]
    public Transform puntoDestino;
    public CanvasGroup panelNegro;
    public float velocidadFade = 2f;
    public AudioSource sonidoPasos;

    [Header("Configuración del Mensaje")]
    public TextMeshProUGUI textoPantalla; // Arrastra el texto de la JERARQUÍA
    public string mensajeAMostrar = "Pulsa [E] para subir";

    private bool enRango = false;
    private bool enTransicion = false;
    private GameObject jugador;

    // ---------------------------------------------------------
    // DETECTAR ENTRADA
    // ---------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("✅ ¡CONTACTO CON ESCALERA!"); // Puedes borrar esto si ya te funciona
            enRango = true;
            jugador = other.gameObject;

            // Solo mostramos el texto si el juego NO está en pausa
            if (textoPantalla != null && Time.timeScale > 0)
            {
                textoPantalla.text = mensajeAMostrar;
                textoPantalla.gameObject.SetActive(true);
            }
        }
    }

    // ---------------------------------------------------------
    // DETECTAR SALIDA
    // ---------------------------------------------------------
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enRango = false;
            jugador = null;

            if (textoPantalla != null)
            {
                textoPantalla.gameObject.SetActive(false);
            }
        }
    }

    // ---------------------------------------------------------
    // BUCLE PRINCIPAL (CON PROTECCIÓN DE PAUSA)
    // ---------------------------------------------------------
    void Update()
    {
        // 1. GESTIÓN INTELIGENTE DEL TEXTO
        if (enRango && textoPantalla != null)
        {
            // Si pusiste PAUSA (TimeScale es 0) -> OCULTA EL TEXTO
            if (Time.timeScale == 0)
            {
                textoPantalla.gameObject.SetActive(false);
            }
            // Si el juego CORRE y no estamos viajando -> MUESTRA EL TEXTO
            else if (!enTransicion)
            {
                // Solo lo activamos si estaba apagado, para no parpadear
                if (!textoPantalla.gameObject.activeSelf)
                    textoPantalla.gameObject.SetActive(true);
            }
        }

        // 2. INPUT (Solo funciona si el tiempo corre)
        if (enRango && !enTransicion && Time.timeScale > 0 && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(SecuenciaSubirEscalera());
        }
    }

    // ---------------------------------------------------------
    // SECUENCIA DE VIAJE
    // ---------------------------------------------------------
    IEnumerator SecuenciaSubirEscalera()
    {
        enTransicion = true;

        // Ocultar texto y empezar sonido
        if (textoPantalla != null) textoPantalla.gameObject.SetActive(false);

        if (sonidoPasos != null)
        {
            sonidoPasos.loop = true;
            sonidoPasos.Play();
        }

        // Fundido a Negro
        while (panelNegro.alpha < 1)
        {
            panelNegro.alpha += Time.deltaTime * velocidadFade;
            yield return null;
        }

        // Teletransporte
        if (jugador != null)
        {
            CharacterController controller = jugador.GetComponent<CharacterController>();
            if (controller != null) controller.enabled = false;

            jugador.transform.position = puntoDestino.position;
            jugador.transform.rotation = puntoDestino.rotation;

            if (controller != null) controller.enabled = true;
        }

        // Reset variables para evitar bucles
        enRango = false;
        jugador = null;

        yield return new WaitForSeconds(0.5f);

        // Fundido a Transparente
        while (panelNegro.alpha > 0)
        {
            panelNegro.alpha -= Time.deltaTime * velocidadFade;
            yield return null;
        }

        // Parar sonido
        if (sonidoPasos != null)
        {
            sonidoPasos.Stop();
        }

        enTransicion = false;
    }
}