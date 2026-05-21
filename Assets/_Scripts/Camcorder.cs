using UnityEngine;
using UnityEngine.UI;

public class Camcorder : MonoBehaviour
{
    [Header("Conexiones Principales")]
    public GameObject nightVisionLight;   // Tu luz verde potente
    public GameObject nightVisionOverlay; // Tu pantalla verde
    public Image batteryUI;

    [Header("Sistema de Emergencia (NUEVO)")]
    public GameObject luzReserva;         // ARRASTRA AQUÍ LA LUZ TENUE ROJA
    public AudioSource audioSource;       // El altavoz de la cámara
    public AudioClip sonidoBateriaBaja;   // Sonido "bip bip"
    public float umbralAviso = 20f;       // A partir de cuánto empieza a pitar

    [Header("Configuración")]
    public float maxBattery = 100f;
    public float drainRate = 10f;

    private float currentBattery;
    private bool isNightVisionOn = false;
    private bool haSonadoAviso = false;   // Para que no pite todo el rato

    // --- NUEVO: Variable para recordar tu color verde ---
    private Color originalColor;

    void Start()
    {
        currentBattery = maxBattery;

        // --- NUEVO: Guardamos el color que pusiste en el editor antes de que cambie ---
        if (batteryUI != null) originalColor = batteryUI.color;

        // Apagamos todo al empezar
        if (nightVisionLight != null) nightVisionLight.SetActive(false);
        if (nightVisionOverlay != null) nightVisionOverlay.SetActive(false);
        if (luzReserva != null) luzReserva.SetActive(false);
    }

    void Update()
    {
        // --- 1. INTERRUPTOR (Tecla F) ---
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleNightVision();
        }

        // --- 2. GASTO DE BATERÍA ---
        if (isNightVisionOn && currentBattery > 0)
        {
            currentBattery -= drainRate * Time.deltaTime;

            // AVISO SONORO (Al 20%)
            if (currentBattery <= umbralAviso && !haSonadoAviso)
            {
                if (audioSource != null && sonidoBateriaBaja != null)
                    audioSource.PlayOneShot(sonidoBateriaBaja);

                haSonadoAviso = true; // Marcamos para que solo suene una vez
            }

            // BATERÍA AGOTADA
            if (currentBattery <= 0)
            {
                currentBattery = 0;
                ForceTurnOff(); // Apaga la nocturna y ENCIENDE la reserva
            }
        }

        // --- 3. ACTUALIZAR BARRA UI ---
        if (batteryUI != null)
        {
            batteryUI.fillAmount = currentBattery / maxBattery;

            // CAMBIO AQUÍ: Usamos originalColor en vez de Color.white
            if (currentBattery <= umbralAviso)
            {
                batteryUI.color = Color.red;
            }
            else
            {
                batteryUI.color = originalColor; // Vuelve a tu verde original
            }
        }
    }

    void ToggleNightVision()
    {
        // Si no hay batería, no dejo encender la nocturna (pero la reserva sigue ahí)
        if (currentBattery <= 0) return;

        isNightVisionOn = !isNightVisionOn;

        if (nightVisionLight != null) nightVisionLight.SetActive(isNightVisionOn);
        if (nightVisionOverlay != null) nightVisionOverlay.SetActive(isNightVisionOn);

        // Si encendemos la nocturna, apagamos la reserva
        if (isNightVisionOn && luzReserva != null) luzReserva.SetActive(false);
    }

    void ForceTurnOff()
    {
        isNightVisionOn = false;

        // Apagamos lo potente
        if (nightVisionLight != null) nightVisionLight.SetActive(false);
        if (nightVisionOverlay != null) nightVisionOverlay.SetActive(false);

        // ENCENDEMOS LA RESERVA (Para que no te quedes ciego)
        if (luzReserva != null) luzReserva.SetActive(true);

        Debug.Log("¡Batería agotada! Activando luz de emergencia.");
    }

    // Función para recargar (usada por las pilas)
    public void RecargarBateria(float cantidad)
    {
        currentBattery += cantidad;
        if (currentBattery > maxBattery) currentBattery = maxBattery;

        // Reseteamos el aviso sonoro
        haSonadoAviso = false;

        // CAMBIO AQUÍ: Al recargar, volvemos al color original
        if (batteryUI != null) batteryUI.color = originalColor;

        // Si recargamos, apagamos la luz de reserva automáticamente
        if (luzReserva != null) luzReserva.SetActive(false);
    }
}