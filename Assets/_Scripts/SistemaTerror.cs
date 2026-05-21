using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SistemaTerror : MonoBehaviour
{
    [Header("Conexiones")]
    public Transform camaraJugador;
    public Transform cabezaEnemigo;
    public Volume volumeGlobal;

    [Header("Configuración")]
    public float distanciaEmpieza = 20f;
    public float distanciaTope = 4f;

    [Header("Ajustes de Mirada")]
    [Range(0.5f, 0.99f)]
    public float precisionNecesaria = 0.85f; // Cuánto tienes que centrar la mirada
    public float velocidadRecuperacion = 2f; // Qué tan rápido se quita la distorsión

    // Variables internas de los efectos
    private FilmGrain grano;
    private ChromaticAberration aberracion;
    private Vignette viñeta;
    private LensDistortion distorsion;

    // Valores originales para volver a ellos
    private float granoBase, aberracionBase, vinetaBase;

    // Variable para suavizado
    private float distorsionActual = 0f;

    void Start()
    {
        if (volumeGlobal == null || volumeGlobal.profile == null)
        {
            Debug.LogError("¡No hay Volume Global asignado en el SistemaTerror!");
            return;
        }

        // Intentamos obtener los efectos del perfil
        volumeGlobal.profile.TryGet(out grano);
        volumeGlobal.profile.TryGet(out aberracion);
        volumeGlobal.profile.TryGet(out viñeta);
        volumeGlobal.profile.TryGet(out distorsion);

        // Guardamos los valores base que tengas en el Volume
        if (grano != null) granoBase = grano.intensity.value;
        if (aberracion != null) aberracionBase = aberracion.intensity.value;
        if (viñeta != null) vinetaBase = viñeta.intensity.value;

        // Inicializamos la distorsión a 0
        if (distorsion != null)
        {
            distorsion.intensity.value = 0f;
            distorsion.scale.value = 1f;
        }
    }

    void Update()
    {
        if (camaraJugador == null || cabezaEnemigo == null) return;

        // 1. CÁLCULO DE DISTANCIA (Para el Ruido/Grano)
        float distanciaReal = Vector3.Distance(camaraJugador.position, cabezaEnemigo.position);
        float factorDistancia = Mathf.InverseLerp(distanciaEmpieza, distanciaTope, distanciaReal);

        // 2. CÁLCULO DE MIRADA (Para la Distorsión de lente)
        Vector3 direccion = (cabezaEnemigo.position - camaraJugador.position).normalized;
        float mirada = Vector3.Dot(camaraJugador.forward, direccion);

        // ¿Lo estamos mirando de frente?
        bool loEstoyMirando = mirada > precisionNecesaria;

        // 3. APLICAR EFECTOS

        // El ruido siempre se aplica si estamos cerca (factorDistancia > 0)
        AplicarRuido(factorDistancia);

        // La distorsión solo si lo miramos Y estamos cerca
        float objetivoDistorsion = loEstoyMirando ? factorDistancia : 0f;
        AplicarDistorsion(objetivoDistorsion);
    }

    void AplicarRuido(float intensidad)
    {
        if (grano != null)
            grano.intensity.value = Mathf.Lerp(granoBase, 1f, intensidad);

        if (aberracion != null)
            aberracion.intensity.value = Mathf.Lerp(aberracionBase, 1f, intensidad);

        if (viñeta != null)
            viñeta.intensity.value = Mathf.Lerp(vinetaBase, 0.65f, intensidad);
    }

    void AplicarDistorsion(float objetivo)
    {
        if (distorsion == null) return;

        // Suavizado del valor actual hacia el objetivo
        distorsionActual = Mathf.Lerp(distorsionActual, objetivo, Time.deltaTime * velocidadRecuperacion);

        // Aplicamos la intensidad (valor negativo para efecto de "succión")
        distorsion.intensity.value = Mathf.Lerp(0f, -0.7f, distorsionActual);

        // Ajustamos la escala para que no se vean bordes negros al distorsionar
        distorsion.scale.value = Mathf.Lerp(1f, 0.9f, distorsionActual);
    }
}