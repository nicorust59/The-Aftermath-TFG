using UnityEngine;


public class SistemaInteraccion : MonoBehaviour
{
    [Header("Configuración")]
    public float distancia = 3.0f;

    [Header("Interfaz Inmersiva")]
    public GameObject puntoMiraGO;  // Arrastra aquí tu "Punto_Mira" (el circulito minúsculo)
    public GameObject iconoManoGO;  // Arrastra aquí el "Agarrar_mira" (la mano que pusiste en el Canvas)

    void Update()
    {
        // Lanzamos el rayo desde el centro de la cámara
        Ray rayo = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit golpe;

        bool heDetectadoAlgo = false;

        // Si el rayo choca con algo...
        if (Physics.Raycast(rayo, out golpe, distancia))
        {
            // Verificamos si ese objeto tiene el script "InteractableObject"
            InteractableObject objetoInteractivo = golpe.transform.GetComponent<InteractableObject>();

            if (objetoInteractivo != null)
            {
                heDetectadoAlgo = true;

                // 1. INTERFAZ: Apagamos el punto y encendemos la mano
                if (puntoMiraGO != null) puntoMiraGO.SetActive(false);
                if (iconoManoGO != null) iconoManoGO.SetActive(true);

                // 2. DETECTAR LA TECLA E
                if (Input.GetKeyDown(KeyCode.E))
                {
                    objetoInteractivo.DoInteraction();

                    // Nota: Si el objeto es una llave y se destruye al cogerla,
                    // en el siguiente frame el rayo ya no chocará con nada
                    // y la mano se apagará automáticamente gracias al código de abajo.
                }
            }
        }

        // Si no miro a nada interactuable (o miro a la pared)...
        if (!heDetectadoAlgo)
        {
            // 3. INTERFAZ: Apagamos la mano y volvemos a encender el punto minúsculo
            if (iconoManoGO != null) iconoManoGO.SetActive(false);
            if (puntoMiraGO != null) puntoMiraGO.SetActive(true);
        }
    }
}