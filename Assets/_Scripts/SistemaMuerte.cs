using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SistemaMuerte : MonoBehaviour
{
    [Header("Conexiones")]
    public Transform jugador;
    public Transform camara;
    public Transform cabezaEnemigo;

    // ¡LA LÍNEA NUEVA PARA TU PANEL!
    public GameObject panelGameOver;

    // YA NO ES PÚBLICO: Lo buscamos solo para que no falle al arrastrar
    private AudioSource audioGrito;

    [Header("Configuración")]
    public float distanciaMuerte = 2.5f;
    public float velocidadGiro = 15f;
    // He subido un poco el tiempo para que te dé tiempo a leer el cartel
    public float tiempoEsperaReinicio = 4.0f;

    private bool estaMuerto = false;
    private MonoBehaviour scriptMovimiento;

    void Start()
    {
        Time.timeScale = 1f;

        // ¡NUEVO! Nos aseguramos de apagar el cartel negro al empezar a jugar
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(false);
        }

        // 1. BUSCAR JUGADOR
        if (jugador == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) jugador = p.transform;
        }

        if (camara == null && Camera.main != null) camara = Camera.main.transform;

        // 2. BUSCAR EL GRITO AUTOMÁTICAMENTE
        if (jugador != null)
        {
            Transform hijoAltavoz = jugador.Find("Altavoz_Grito");
            if (hijoAltavoz != null)
            {
                audioGrito = hijoAltavoz.GetComponent<AudioSource>();
            }
            else
            {
                audioGrito = jugador.GetComponentInChildren<AudioSource>();
            }
        }

        // 3. BUSCAR MOVIMIENTO
        if (jugador != null)
        {
            var scripts = jugador.GetComponents<MonoBehaviour>();
            foreach (var s in scripts)
            {
                if (s.GetType().Name.Contains("Controller") || s.GetType().Name.Contains("Move"))
                    scriptMovimiento = s;
            }
        }
    }

    void Update()
    {
        if (estaMuerto || jugador == null || cabezaEnemigo == null) return;

        float distancia = Vector3.Distance(jugador.position, cabezaEnemigo.position);

        if (distancia < distanciaMuerte)
        {
            StartCoroutine(SecuenciaMuerte());
        }
    }

    IEnumerator SecuenciaMuerte()
    {
        estaMuerto = true;
        Time.timeScale = 1f;

        // Bloquear movimiento
        if (jugador != null)
        {
            var controles = jugador.GetComponentsInChildren<MonoBehaviour>();
            foreach (var c in controles)
            {
                if (c.GetType().Name.Contains("Controller") || c.GetType().Name.Contains("Move") || c.GetType().Name.Contains("Input"))
                    c.enabled = false;
            }
        }

        // REPRODUCIR SONIDO (Si lo encontró)
        if (audioGrito != null)
        {
            audioGrito.Play();
        }
        else
        {
            Debug.LogWarning("⚠️ No encontré el AudioSource, pero te mato igual.");
        }

        // Giro de Cámara (¡El Jumpscare!)
        float tiempo = 0f;
        while (tiempo < 1.5f)
        {
            Vector3 direccionCara = cabezaEnemigo.position - camara.position;
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionCara);
            camara.rotation = Quaternion.Slerp(camara.rotation, rotacionObjetivo, Time.unscaledDeltaTime * velocidadGiro);

            tiempo += Time.unscaledDeltaTime;
            yield return null;
        }

        // ¡NUEVO! Encender la pantalla de "HAS MUERTO" después del susto
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
        }

        // Esperamos un par de segundos viendo la pantalla antes de reiniciar
        yield return new WaitForSeconds(tiempoEsperaReinicio - 1.5f);
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