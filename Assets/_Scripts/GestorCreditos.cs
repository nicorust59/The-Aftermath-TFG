using UnityEngine;

public class GestorCreditos : MonoBehaviour
{
    [Header("Configuración del Texto")]
    public RectTransform textoMoviendose;
    public float velocidadSubida = 100f;
    public float duracionCreditos = 25f;

    [Header("Configuración de Música")]
    public AudioSource reproductorMusica;
    public AudioClip cancionFinal;

    [Header("El Gran Final")]
    public GameObject panelVictoria;

    private bool creditosActivos = false;

    // Se ejecuta cuando el Timeline enciende este objeto
    void OnEnable()
    {
        Debug.Log("🟩 Iniciando secuencia de créditos...");

        // 1. ENCENDER EL TEXTO (Por si estaba apagado)
        if (textoMoviendose != null)
        {
            textoMoviendose.gameObject.SetActive(true);
        }

        // 2. REPRODUCIR MÚSICA
        if (reproductorMusica != null && cancionFinal != null)
        {
            reproductorMusica.clip = cancionFinal;
            reproductorMusica.loop = false;
            reproductorMusica.Play();
        }

        creditosActivos = true;
        Invoke("FinalizarCreditos", duracionCreditos);
    }

    void Update()
    {
        // Movimiento robusto para UI
        if (creditosActivos && textoMoviendose != null)
        {
            textoMoviendose.anchoredPosition += Vector2.up * velocidadSubida * Time.deltaTime;
        }
    }

    void FinalizarCreditos()
    {
        creditosActivos = false;
        if (reproductorMusica != null) reproductorMusica.Stop();
        if (panelVictoria != null) panelVictoria.SetActive(true);

        // Nos apagamos para no molestar más
        gameObject.SetActive(false);
    }
}