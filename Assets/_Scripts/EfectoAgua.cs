using UnityEngine;

public class EfectoAgua : MonoBehaviour
{
    [Header("Configuración")]
    public Transform camaraJugador; // Arrastra tu cámara aquí en el Inspector
    public float cantidadBalanceo = 0.15f; // Cuánto sube y baja la cabeza
    public float velocidadOlas = 2f;       // Lo rápido que sube y baja

    private bool estaEnAgua = false;
    private Vector3 posicionOriginalCamara;
    private float temporizador = 0f;

    void Start()
    {
        // Guardamos la altura original de la cámara (los "ojos" del jugador)
        if (camaraJugador != null)
        {
            posicionOriginalCamara = camaraJugador.localPosition;
        }
    }

    void Update()
    {
        if (camaraJugador == null) return;

        if (estaEnAgua)
        {
            // EL TRUCO DE LA OLA: Usamos la función matemática Seno (Sin) para subir y bajar suavemente
            temporizador += Time.deltaTime * velocidadOlas;
            float nuevaY = posicionOriginalCamara.y + Mathf.Sin(temporizador) * cantidadBalanceo;

            // Aplicamos el movimiento a la cámara
            camaraJugador.localPosition = new Vector3(camaraJugador.localPosition.x, nuevaY, camaraJugador.localPosition.z);
        }
        else
        {
            // Si salimos del agua, devolvemos la cámara a su sitio suavemente
            camaraJugador.localPosition = Vector3.Lerp(camaraJugador.localPosition, posicionOriginalCamara, Time.deltaTime * 5f);
            temporizador = 0f;
        }
    }

    // --- DETECCIÓN DE ENTRADA AL AGUA ---
    private void OnTriggerEnter(Collider other)
    {
        // Si tocamos el cubo invisible con el tag Agua, activamos el balanceo
        if (other.CompareTag("Agua"))
        {
            estaEnAgua = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si salimos del cubo invisible, lo apagamos
        if (other.CompareTag("Agua"))
        {
            estaEnAgua = false;
        }
    }
}