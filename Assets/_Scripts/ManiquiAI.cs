using UnityEngine;
using UnityEngine.AI;

public class ManiquiAI : MonoBehaviour
{
    [Header("Configuración Básica")]
    public Transform jugador;      // Arrastra a tu Player aquí
    public Camera camaraJugador;   // Arrastra tu Main Camera aquí
    public float velocidad = 7.0f; // Rápido para dar miedo

    // --- LÍNEA NUEVA: El cerebro de las animaciones ---
    public Animator animador;

    [Header("Audio del Terror")]
    public AudioSource fuenteAudio; // El altavoz del maniquí
    public AudioClip sonidoPasos;   // Sonido de pasos rápidos o crujidos

    [Header("Ajustes Técnicos")]
    public float distanciaMaximaVision = 20f;

    private NavMeshAgent agente;
    private Renderer miRenderer;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        // Si se nos olvidó arrastrar el Animator, lo busca solo
        if (animador == null)
        {
            animador = GetComponent<Animator>();
        }

        // 1. Buscamos la piel del modelo (por si es un FBX importado)
        miRenderer = GetComponentInChildren<Renderer>();
        if (miRenderer == null)
        {
            Debug.LogError("¡ERROR! No encuentro el Renderer en los hijos.");
        }

        // --- CORRECCIÓN DE SUELO (EL ARREGLO NUEVO) ---
        // Desactivamos el agente un momento para moverlo sin errores
        agente.enabled = false;

        // Buscamos el punto válido del suelo más cercano (radio de 5 metros)
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 5.0f, NavMesh.AllAreas))
        {
            // ¡Lo pegamos al suelo a la fuerza!
            transform.position = hit.position;
            Debug.Log("✅ Maniquí pegado al suelo correctamente.");
        }
        else
        {
            Debug.LogError("❌ ERROR CRÍTICO: El maniquí está demasiado lejos del NavMesh (suelo azul).");
        }

        // Volvemos a encenderlo ya colocado
        agente.enabled = true;
        // ---------------------------------------------

        // Configuramos la velocidad
        agente.speed = velocidad;

        // Auto-asignaciones de seguridad
        if (camaraJugador == null) camaraJugador = Camera.main;
        if (fuenteAudio == null) fuenteAudio = GetComponent<AudioSource>();

        if (fuenteAudio != null && sonidoPasos != null)
        {
            fuenteAudio.clip = sonidoPasos;
        }
    }

    void Update()
    {
        if (jugador == null) return;

        bool esVisto = EstaSiendoVisto();

        if (esVisto)
        {
            // --- MODO ESTATUA (Te están mirando) ---
            if (agente.isOnNavMesh) // Seguridad extra
            {
                agente.isStopped = true;
                agente.velocity = Vector3.zero;
            }

            // --- LÍNEA NUEVA: Le decimos al Animator que pare de correr ---
            if (animador != null)
            {
                animador.SetBool("estaMoviendo", false);
            }

            if (fuenteAudio != null && fuenteAudio.isPlaying)
            {
                fuenteAudio.Pause();
            }
        }
        else
        {
            // --- MODO CAZADOR (Nadie mira) ---
            if (agente.isOnNavMesh) // Seguridad extra
            {
                agente.isStopped = false;
                agente.SetDestination(jugador.position);
            }

            // --- LÍNEA NUEVA: Le decimos al Animator que corra ---
            if (animador != null)
            {
                animador.SetBool("estaMoviendo", true);
            }

            if (fuenteAudio != null && !fuenteAudio.isPlaying)
            {
                fuenteAudio.Play();
            }
        }
    }

    bool EstaSiendoVisto()
    {
        if (miRenderer == null) return false;

        Plane[] planos = GeometryUtility.CalculateFrustumPlanes(camaraJugador);
        if (GeometryUtility.TestPlanesAABB(planos, miRenderer.bounds))
        {
            RaycastHit hit;
            Vector3 direccion = jugador.position - transform.position;

            // Rayo desde un poco más arriba (Vector3.up)
            if (Physics.Raycast(transform.position + Vector3.up, direccion.normalized, out hit, distanciaMaximaVision))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }
}