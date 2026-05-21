using UnityEngine;
using System.Collections;
using TMPro;

public class NotaInteractuable : MonoBehaviour
{
    [Header("Lo que dice ESTA nota")]
    [TextArea(5, 10)]
    public string contenidoNota = "Escribe aquí tu historia de terror...";

    [Header("Configuración Visor")]
    public GameObject panelVisorNota;   // El panel de papel
    public TextMeshProUGUI textoVisor;  // El texto negro de dentro
    public GameObject textoAviso;       // El mensaje "Pulsa E para leer"

    [Header("Efecto Mecanografía")]
    public float velocidadEscritura = 0.05f; // Tiempo entre letras
    public AudioSource audioEscritura;       // Arrastra aquí el sonido de la tecla

    private bool cerca = false;
    private bool leyendo = false;
    private bool escribiendo = false;
    private Coroutine rutinaEscritura;

    void Start()
    {
        if (panelVisorNota != null) panelVisorNota.SetActive(false);
    }

    void Update()
    {
        // 1. ABRIR NOTA (Solo con E, como interactuar normal)
        if (cerca && !leyendo && Input.GetKeyDown(KeyCode.E))
        {
            AbrirNota();
        }
        // 2. INTERACTUAR MIENTRAS SE LEE (Espacio, E o Escape)
        else if (leyendo)
        {
            // Comprobamos si pulsa cualquiera de las 3 teclas
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
            {
                // CASO A: Si la máquina aún está escribiendo -> TERMINAR DE GOLPE
                if (escribiendo)
                {
                    CompletarDeGolpe();
                }
                // CASO B: Si ya acabó de escribir -> CERRAR NOTA
                else
                {
                    CerrarNota();
                }
            }
        }
    }

    void AbrirNota()
    {
        leyendo = true;

        if (panelVisorNota != null) panelVisorNota.SetActive(true);
        if (textoVisor != null) textoVisor.text = "";

        if (textoAviso != null) textoAviso.SetActive(false);

        // Pausa y Ratón
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        rutinaEscritura = StartCoroutine(EscribirTexto());
    }

    IEnumerator EscribirTexto()
    {
        escribiendo = true;

        if (audioEscritura != null) audioEscritura.Play();

        foreach (char letra in contenidoNota.ToCharArray())
        {
            if (textoVisor != null)
            {
                textoVisor.text += letra;
                // Variación de tono
                if (audioEscritura != null) audioEscritura.pitch = Random.Range(0.9f, 1.1f);
            }

            yield return new WaitForSecondsRealtime(velocidadEscritura);
        }

        FinalizarEscritura();
    }

    void CompletarDeGolpe()
    {
        if (rutinaEscritura != null) StopCoroutine(rutinaEscritura);

        if (textoVisor != null) textoVisor.text = contenidoNota;

        FinalizarEscritura();
    }

    void FinalizarEscritura()
    {
        escribiendo = false;
        if (audioEscritura != null) audioEscritura.Stop();
    }

    void CerrarNota()
    {
        leyendo = false;
        escribiendo = false;

        if (rutinaEscritura != null) StopCoroutine(rutinaEscritura);
        if (audioEscritura != null) audioEscritura.Stop();

        if (panelVisorNota != null) panelVisorNota.SetActive(false);
        if (textoAviso != null) textoAviso.SetActive(true);

        // Volver al juego normal
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void AbrirDesdeMano()
    {
        // Llamamos a tu función de abrir que ya funciona
        if (!leyendo)
        {
            AbrirNota();
        }
    }

    // --- DETECCION ---
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cerca = true;
            if (textoAviso != null) textoAviso.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cerca = false;
            if (textoAviso != null) textoAviso.SetActive(false);
            if (leyendo) CerrarNota();
        }
    }
}