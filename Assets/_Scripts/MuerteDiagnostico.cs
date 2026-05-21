using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MuerteDiagnostico : MonoBehaviour
{
    [Header("Interfaz (Game Over)")]
    public GameObject pantallaGameOver; // Arrastra el panel negro aquí

    [Header("Conexiones")]
    public Transform jugador;
    public Transform camara;
    public Transform cabezaEnemigo;

    [Header("Audio")]
    public AudioSource audioGrito; // Arrastra el hijo "Altavoz_Grito"

    [Header("Ajustes")]
    public float distanciaMuerte = 2.5f;
    public float velocidadGiro = 15f;
    public float tiempoLectura = 3.0f;

    private bool estaMuerto = false;
    private MonoBehaviour scriptMovimiento;

    void Start()
    {
        // Aseguramos que el juego empiece corriendo
        Time.timeScale = 1f;

        // --- AUTO-BÚSQUEDAS ---
        if (jugador == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) jugador = p.transform;
        }
        if (camara == null && Camera.main != null) camara = Camera.main.transform;

        // Buscar script de movimiento para bloquearlo luego
        if (jugador != null) scriptMovimiento = jugador.GetComponent<MonoBehaviour>();
    }

    void Update()
    {
        // 1. PROTECCIÓN DE VICTORIA Y MUERTE
        // Si el tiempo es 0 (porque ganaste) O ya estás muerto, NO hacemos nada.
        if (Time.timeScale == 0f || estaMuerto) return;

        if (jugador == null || cabezaEnemigo == null) return;

        // 2. MEDIR DISTANCIA
        float distancia = Vector3.Distance(jugador.position, cabezaEnemigo.position);

        // Mensaje en consola (como pediste)
        Debug.Log($"Distancia: {distancia:F2}");

        if (distancia < distanciaMuerte)
        {
            // 3. CANDADO: Bloqueamos inmediatamente para que no se repita
            estaMuerto = true;
            Debug.Log("💀 ¡CONTACTO! Iniciando muerte y silenciando todo...");
            StartCoroutine(SecuenciaMuerte());
        }
    }

    IEnumerator SecuenciaMuerte()
    {
        // Aseguramos que la animación corra aunque vengas de una pausa
        Time.timeScale = 1f;

        // A. BLOQUEO DE MOVIMIENTO DEL JUGADOR
        if (jugador != null)
        {
            var controles = jugador.GetComponentsInChildren<MonoBehaviour>();
            foreach (var c in controles)
            {
                // Desactiva cualquier cosa que parezca un control
                if (c.GetType().Name.Contains("Controller") || c.GetType().Name.Contains("Move") || c.GetType().Name.Contains("Input"))
                    c.enabled = false;
            }
        }

        // B. BOMBA DE SILENCIO 🔇💣
        // Busca TODOS los audios del juego
        AudioSource[] todosLosSonidos = FindObjectsOfType<AudioSource>();
        foreach (AudioSource sonido in todosLosSonidos)
        {
            // Si NO es el grito, lo apagamos.
            if (sonido != audioGrito)
            {
                sonido.Stop();
                sonido.enabled = false;
            }
        }

        // C. GRITO (El único que suena)
        if (audioGrito != null) audioGrito.Play();

        // D. GIRO DE CÁMARA (Susto visual)
        float tiempo = 0f;
        while (tiempo < 1.5f)
        {
            Vector3 direccionCara = cabezaEnemigo.position - camara.position;
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionCara);
            camara.rotation = Quaternion.Slerp(camara.rotation, rotacionObjetivo, Time.unscaledDeltaTime * velocidadGiro);

            tiempo += Time.unscaledDeltaTime;
            yield return null;
        }

        // E. MOSTRAR PANTALLA GAME OVER
        if (pantallaGameOver != null)
        {
            pantallaGameOver.SetActive(true);

            // Soltamos el ratón por si acaso
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // F. ESPERA DRAMÁTICA
        yield return new WaitForSeconds(tiempoLectura);

        // G. REINICIAR NIVEL
        Debug.Log("🔄 Reiniciando...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDrawGizmos()
    {
        if (cabezaEnemigo != null)
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawSphere(cabezaEnemigo.position, distanciaMuerte);
        }
    }
}