using UnityEngine;

public class ScrollCreditos : MonoBehaviour
{
    [Header("Configuración de Créditos")]
    [Tooltip("Velocidad a la que sube el texto. Un número más alto = más rápido.")]
    public float velocidad = 50f;

    private RectTransform rectTransform;

    void Start()
    {
        // Obtenemos el componente que controla la posición en la interfaz (UI)
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Movemos el texto hacia arriba constantemente en el eje Y
        rectTransform.anchoredPosition += Vector2.up * velocidad * Time.deltaTime;
    }
}