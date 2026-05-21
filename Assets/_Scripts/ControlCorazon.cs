using UnityEngine;

public class ControlCorazon : MonoBehaviour
{
    [Header("Conexiones")]
    public Transform jugador;      // Tu Player
    public Transform enemigo;      // Tu Enemigo_Padre
    public AudioSource audioSource; // El altavoz que pusiste en el Paso 1

    [Header("Configuración de Pánico")]
    public float distanciaEmpieza = 10f; // A 10m empieza a sonar
    public float distanciaMaxima = 2f;   // A 2m está a tope (pegado a ti)

    [Range(1f, 3f)]
    public float velocidadMaxima = 2.0f; // Qué tan rápido late al final (2.0 = Doble de rápido)

    void Update()
    {
        if (jugador == null || enemigo == null || audioSource == null) return;

        // 1. Calculamos la distancia
        float distancia = Vector3.Distance(jugador.position, enemigo.position);

        // 2. Convertimos distancia en Intensidad (0 a 1)
        // InverseLerp hace la magia: 
        // Si distancia es 10 -> devuelve 0.
        // Si distancia es 2 -> devuelve 1.
        float intensidad = Mathf.InverseLerp(distanciaEmpieza, distanciaMaxima, distancia);

        // 3. Aplicamos al Audio
        // El volumen sube según la intensidad
        audioSource.volume = intensidad;

        // El Pitch (velocidad) va de 1 (normal) a 'velocidadMaxima' (taquicardia)
        audioSource.pitch = Mathf.Lerp(1f, velocidadMaxima, intensidad);
    }
}