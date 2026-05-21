using UnityEngine;

public class ZonaDeAgua : MonoBehaviour
{
    [Header("Configuración de Nado")]
    public float velocidadNadoArriba = 15f; // HE CAMBIADO EL DEFECTO A 15
    public string tagJugador = "Player";

    [Header("Efectos Visuales (Niebla)")]
    public bool activarNiebla = true;
    public Color colorAgua = new Color(0.2f, 0.4f, 0.6f, 0.5f);
    public float densidadNiebla = 0.15f;

    private bool nieblaOriginalActiva;
    private Color colorOriginal;
    private float densidadOriginal;

    [Header("Sonido")]
    public AudioSource sonidoBajoAgua;

    private CharacterController controladorJugador;
    private bool jugadorEnAgua = false;

    void Start()
    {
        nieblaOriginalActiva = RenderSettings.fog;
        colorOriginal = RenderSettings.fogColor;
        densidadOriginal = RenderSettings.fogDensity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagJugador))
        {
            jugadorEnAgua = true;
            controladorJugador = other.GetComponent<CharacterController>();

            if (activarNiebla)
            {
                RenderSettings.fog = true;
                RenderSettings.fogColor = colorAgua;
                RenderSettings.fogDensity = densidadNiebla;
                RenderSettings.fogMode = FogMode.Exponential;
            }

            if (sonidoBajoAgua != null) sonidoBajoAgua.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagJugador))
        {
            jugadorEnAgua = false;
            controladorJugador = null;

            RenderSettings.fog = nieblaOriginalActiva;
            RenderSettings.fogColor = colorOriginal;
            RenderSettings.fogDensity = densidadOriginal;

            if (sonidoBajoAgua != null) sonidoBajoAgua.Stop();
        }
    }

    void Update()
    {
        if (jugadorEnAgua && controladorJugador != null)
        {
            // AHORA USAMOS GetKey (MANTENER)
            if (Input.GetKey(KeyCode.Space))
            {
                // Movemos hacia arriba con fuerza bruta
                Vector3 nado = Vector3.up * velocidadNadoArriba * Time.deltaTime;
                controladorJugador.Move(nado);
            }
        }
    }
}