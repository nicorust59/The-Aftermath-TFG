using UnityEngine;

public class ClimaxCthulhu : MonoBehaviour
{
    [Header("El Despertar")]
    public GameObject modeloCthulhu; // Arrastra a tu Cthulhu gigante aquí

    [Header("Audio Dinámico")]
    public AudioSource musicaAmbiente;    // La música tranquila (para apagarla)
    public AudioSource musicaPersecucion; // La música rápida de huida
    public AudioSource sonidoRugido;      // El grito de Cthulhu

    private bool climaxActivado = false;

    void Start()
    {
        // Por seguridad, nos aseguramos de que Cthulhu esté invisible al empezar
        if (modeloCthulhu != null)
        {
            modeloCthulhu.SetActive(false);
        }
    }

    // Esta función detecta si el jugador toca el artefacto
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !climaxActivado)
        {
            ActivarClimax();
        }
    }

    public void ActivarClimax()
    {
        climaxActivado = true;
        Debug.Log("🦑 ¡CTHULHU HA DESPERTADO!");

        // 1. Aparece el Dios en el cielo
        if (modeloCthulhu != null)
            modeloCthulhu.SetActive(true);

        // 2. Suena el rugido ensordecedor
        if (sonidoRugido != null)
            sonidoRugido.Play();

        // 3. Cambio de música (Dynamic Audio)
        if (musicaAmbiente != null)
            musicaAmbiente.Stop();

        if (musicaPersecucion != null)
            musicaPersecucion.Play();

        Collider miColision = GetComponent<Collider>();
        if (miColision != null) miColision.enabled = false;

        Renderer[] misMallas = GetComponentsInChildren<Renderer>();
        foreach (Renderer malla in misMallas)
        {
            malla.enabled = false;
        }
    }
}