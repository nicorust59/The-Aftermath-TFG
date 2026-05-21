using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Paneles")]
    public GameObject pausePanel;

    [Header("Punto de Mira")]
    public GameObject crosshair;

    public static bool isPaused = false;

    // --- BLOQUEO PARA EL FINAL ---
    public static bool finalAlcanzado = false;

    void Start()
    {
        // Al empezar la escena, reseteamos el bloqueo por si acaso
        finalAlcanzado = false;

        if (pausePanel != null) pausePanel.SetActive(false);
        Resume();
    }

    void Update()
    {

        if (PauseMenu.isPaused) return;
        // Si hemos llegado al final, ignoramos el teclado y salimos del Update
        if (finalAlcanzado) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (crosshair != null) crosshair.SetActive(true);

        Time.timeScale = 1f;
        isPaused = false;
        AudioListener.pause = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        if (pausePanel != null) pausePanel.SetActive(true);
        if (crosshair != null) crosshair.SetActive(false);

        Time.timeScale = 0f;
        isPaused = true;
        AudioListener.pause = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        AudioListener.pause = false;
        SceneManager.LoadScene("Menu");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        isPaused = false;
        AudioListener.pause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}