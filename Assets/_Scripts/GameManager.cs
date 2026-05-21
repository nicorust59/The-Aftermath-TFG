using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public int keysCollected = 0;
    public int totalKeys = 3;

    [Header("Variables de Interfaz (UI)")]
    public TextMeshProUGUI keysText;
    public GameObject winScreen;
    public GameObject camaraUI;
    public GameObject iconoMano;

    [Header("Nuevo Feedback de Llaves")]
    public GameObject objetoTextoPensamiento; // El objeto de UI con el texto
    public float duracionMensaje = 4.0f;      // Cuánto tiempo se queda en pantalla

    [Header("Cinemática Final")]
    public PlayableDirector directorFinal;
    public float duracionCinematica = 10.0f;

    void Start()
    {
        UpdateUI();
        if (winScreen != null) winScreen.SetActive(false);
        if (camaraUI != null) camaraUI.SetActive(true);

        // Aseguramos que el pensamiento empiece oculto
        if (objetoTextoPensamiento != null) objetoTextoPensamiento.SetActive(false);
    }

    public void AddKey()
    {
        keysCollected++;
        UpdateUI();
        Debug.Log("Llevas " + keysCollected + " llaves.");

        // SI RECOGE LA ÚLTIMA LLAVE -> LANZAMOS EL PENSAMIENTO
        if (keysCollected == totalKeys)
        {
            StartCoroutine(MostrarPensamiento());
        }
    }

    void UpdateUI()
    {
        if (keysText != null)
            keysText.text = "Llaves: " + keysCollected + " / " + totalKeys;
    }

    public bool HasAllKeys()
    {
        return keysCollected >= totalKeys;
    }

    public void WinGame()
    {
        Debug.Log("¡JUEGO TERMINADO! Iniciando huida...");
        StartCoroutine(SecuenciaVictoria());
    }

    // --- CORRUTINA PARA EL TEXTO DEL PENSAMIENTO ---
    IEnumerator MostrarPensamiento()
    {
        if (objetoTextoPensamiento != null)
        {
            objetoTextoPensamiento.SetActive(true);

            // Esperamos el tiempo que hayas puesto en el Inspector
            yield return new WaitForSeconds(duracionMensaje);

            objetoTextoPensamiento.SetActive(false);
        }
    }

    IEnumerator SecuenciaVictoria()
    {
        // 1. Apagamos TODO lo que estorba en pantalla
        if (camaraUI != null) camaraUI.SetActive(false);
        if (iconoMano != null) iconoMano.SetActive(false);

        // Por si acaso el pensamiento sigue en pantalla, lo apagamos
        if (objetoTextoPensamiento != null) objetoTextoPensamiento.SetActive(false);

        // 2. ACTIVACIÓN DEL GESTOR FINAL
        if (directorFinal != null)
        {
            directorFinal.gameObject.SetActive(true);
            directorFinal.Play();
        }

        // 3. Esperamos la película
        yield return new WaitForSeconds(duracionCinematica);

        // 4. Pantalla de victoria y ratón
        if (winScreen != null) winScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }
}