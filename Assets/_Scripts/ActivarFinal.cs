using UnityEngine;

public class ActivarFinal : MonoBehaviour
{
    public GameObject objetoFinal; // Arrastra aquí el GestorFinal_NUEVO

    void OnTriggerEnter(Collider other)
    {
        // Si el Player toca este cubo invisible...
        if (other.CompareTag("Player"))
        {
            // Encendemos el objeto del final y el Timeline empezará ahora
            objetoFinal.SetActive(true);

            // Opcional: Destruimos este trigger para que no se repita
            Destroy(gameObject);
        }
    }
}