using UnityEngine;

public class SwayBrazo : MonoBehaviour
{
    [Header("Ajustes de Balanceo")]
    public float intensidad = 1.2f;
    public float suavizado = 8f;
    public float intensidadMaxima = 2f;

    private Quaternion rotacionOriginal;

    void Start()
    {
        // Guardamos la rotación que tiene el brazo al empezar
        rotacionOriginal = transform.localRotation;
    }

    void Update()
    {
        // 1. Capturamos el movimiento del ratón
        float mouseX = Input.GetAxis("Mouse X") * intensidad;
        float mouseY = Input.GetAxis("Mouse Y") * intensidad;

        // Limitamos el balanceo para que no se dé la vuelta el brazo
        mouseX = Mathf.Clamp(mouseX, -intensidadMaxima, intensidadMaxima);
        mouseY = Mathf.Clamp(mouseY, -intensidadMaxima, intensidadMaxima);

        // 2. Calculamos la rotación de destino (hacia donde se inclina)
        Quaternion rotacionX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotacionY = Quaternion.AngleAxis(mouseX, Vector3.up);
        Quaternion rotacionDestino = rotacionOriginal * rotacionX * rotacionY;

        // 3. Aplicamos la rotación suavemente (el truco está en el Lerp)
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotacionDestino, suavizado * Time.deltaTime);
    }
}