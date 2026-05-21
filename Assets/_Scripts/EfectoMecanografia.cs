using UnityEngine;
using TMPro;
using System.Collections;

public class EfectoMecanografia : MonoBehaviour
{
    [Header("Configuración")]
    public TMP_Text etiquetaTexto;
    public float velocidadEscritura = 0.04f;
    public AudioSource audioSource;

    [Header("La Historia")]
    [TextArea(10, 20)]
    public string historiaCompleta;

    [Header("Botón Continuar")]
    public GameObject botonEmpezar; // El botón que aparece al final

    private bool estaEscribiendo = false;

    private void OnEnable()
    {
        // Al encenderse, empieza la magia
        StartCoroutine(EscribirTexto());
    }

    void Update()
    {
        // SI PULSAS ESPACIO Y AÚN ESTÁ ESCRIBIENDO...
        if (Input.GetKeyDown(KeyCode.Space) && estaEscribiendo)
        {
            CompletarDeGolpe();
        }
    }

    IEnumerator EscribirTexto()
    {
        estaEscribiendo = true;
        etiquetaTexto.text = ""; // Limpiamos
        botonEmpezar.SetActive(false); // Ocultamos botón

        foreach (char letra in historiaCompleta)
        {
            etiquetaTexto.text += letra;

            // Sonido suave (opcional)
            if (audioSource != null && !audioSource.isPlaying)
                audioSource.Play();

            yield return new WaitForSeconds(velocidadEscritura);
        }

        // Cuando acaba el bucle normal:
        Finalizar();
    }

    void CompletarDeGolpe()
    {
        StopAllCoroutines(); // Detiene el escriba letra a letra
        etiquetaTexto.text = historiaCompleta; // Pone todo el texto de una
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        Finalizar(); // Muestra el botón
    }

    void Finalizar()
    {
        estaEscribiendo = false;
        botonEmpezar.SetActive(true); // ¡Aparece el botón para jugar!
    }
}