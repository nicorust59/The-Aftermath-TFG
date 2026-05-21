using UnityEngine;
using System.Collections;
using TMPro;

public class EfectoHistoria : MonoBehaviour
{
    [Header("Ajustes")]
    public float velocidadEscritura = 0.05f;
    public AudioSource sonidoTeclas;

    private TextMeshProUGUI componenteTexto;
    private string textoCopiado; // Aquí guardaremos lo que ya escribiste en Unity
    private bool escribiendo = false;
    private Coroutine rutina;

    void Awake()
    {
        componenteTexto = GetComponent<TextMeshProUGUI>();

        // PASO CLAVE: Copiamos el texto que ya pusiste en el Inspector de TextMeshPro
        if (componenteTexto != null)
        {
            textoCopiado = componenteTexto.text;
            componenteTexto.text = ""; // Lo vaciamos para empezar de cero
        }
    }

    void OnEnable()
    {
        if (componenteTexto != null && !string.IsNullOrEmpty(textoCopiado))
        {
            SilenciarTodo();
            componenteTexto.text = ""; // Aseguramos que empiece vacío
            rutina = StartCoroutine(Escribir());
        }
    }

    void Update()
    {
        // Si pulsas Espacio, sale todo el texto que habia copiado
        if (Input.GetKeyDown(KeyCode.Space) && escribiendo)
        {
            StopCoroutine(rutina);
            componenteTexto.text = textoCopiado;
            FinalizarEscritura();
        }
    }

    IEnumerator Escribir()
    {
        escribiendo = true;
        if (sonidoTeclas != null) sonidoTeclas.Play();

        foreach (char letra in textoCopiado.ToCharArray())
        {
            componenteTexto.text += letra;
            yield return new WaitForSecondsRealtime(velocidadEscritura);
        }

        FinalizarEscritura();
    }

    void FinalizarEscritura()
    {
        escribiendo = false;
        if (sonidoTeclas != null) sonidoTeclas.Stop();
    }

    void SilenciarTodo()
    {
        // Esto apaga los susurros y sonidos de ambiente
        AudioSource[] todos = FindObjectsOfType<AudioSource>();
        foreach (AudioSource s in todos)
        {
            // No apagamos ni las teclas ni la música de los créditos
            if (s != sonidoTeclas && s.gameObject.name != "Manager_Creditos_Script")
            {
                s.Stop();
            }
        }
    }
}