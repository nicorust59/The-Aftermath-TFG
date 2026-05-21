using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configuración Movimiento")]
    public float speed = 5f;
    public float runSpeed = 10f;
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;

    [Header("Configuración Audio")]
    public AudioSource footstepSource;    // Altavoz de los pies
    public AudioSource breathingSource;   // Altavoz de la boca
    public AudioClip[] footstepSounds;
    public float footstepInterval = 0.5f;

    [Header("Efecto Head Bob")]
    public bool enableHeadBob = true;
    public float bobAmplitude = 0.05f;
    public float bobFrequency = 10f;
    public float runBobMultiplier = 1.5f;

    private CharacterController controller;
    private float xRotation = 0f;
    private float footstepTimer;
    private float defaultYPos = 0;
    private float timer = 0;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        // Si tienes menú, lee la sensibilidad, si no, usa la por defecto
        // if (MainMenuPro.sensibilidadGuardada > 0) mouseSensitivity = MainMenuPro.sensibilidadGuardada;

        if (cameraTransform != null) defaultYPos = cameraTransform.localPosition.y;
    }

    void Update()
    {
        // --- AQUÍ ESTÁ EL CAMBIO PARA LA PAUSA ---
        // Si el menú de pausa dice que está activo, dejamos de leer el ratón y el teclado.
        // Importante: Asegúrate de que tu script de menú se llama exactamente "PauseMenu".
        if (PauseMenu.isPaused) return;
        // -----------------------------------------

        // 1. ROTACIÓN
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);

        // 2. MOVIMIENTO
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        float currentSpeed = isRunning ? runSpeed : speed;

        controller.SimpleMove(move * currentSpeed);

        // 3. HEAD BOB
        if (enableHeadBob && cameraTransform != null)
        {
            if (move.magnitude > 0.1f && controller.isGrounded)
            {
                timer += Time.deltaTime * (isRunning ? bobFrequency * runBobMultiplier : bobFrequency);
                float newY = defaultYPos + Mathf.Sin(timer) * (isRunning ? bobAmplitude * runBobMultiplier : bobAmplitude);
                cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, newY, cameraTransform.localPosition.z);
            }
            else
            {
                timer = 0;
                Vector3 newPos = new Vector3(cameraTransform.localPosition.x, defaultYPos, cameraTransform.localPosition.z);
                cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newPos, Time.deltaTime * 5f);
            }
        }

        // 4. SONIDO DE PASOS
        if (move.magnitude > 0.1f && controller.isGrounded)
        {
            footstepTimer -= Time.deltaTime;
            float currentInterval = isRunning ? 0.3f : footstepInterval; // Pasos más rápidos al correr

            if (footstepTimer <= 0)
            {
                PlayFootstep();
                footstepTimer = currentInterval;
            }
        }
        else
        {
            footstepTimer = 0.1f;
        }

        // 5. SISTEMA DE RESPIRACIÓN
        GestionarRespiracion(move.magnitude > 0.1f, isRunning);
    }

    void GestionarRespiracion(bool isMoving, bool isRunning)
    {
        if (breathingSource == null) return;

        // Si nos movemos Y corremos -> Respirar fuerte
        if (isMoving && isRunning)
        {
            if (!breathingSource.isPlaying)
            {
                breathingSource.Play();
            }
            // Hacemos que el volumen suba suavemente (Fade In)
            if (breathingSource.volume < 1f) breathingSource.volume += Time.deltaTime;
        }
        else
        {
            // Si paramos o andamos despacio -> Bajar volumen suavemente (Fade Out)
            if (breathingSource.volume > 0f)
            {
                breathingSource.volume -= Time.deltaTime * 2f; // Se calla rápido
            }
            else
            {
                breathingSource.Stop();
            }
        }
    }

    void PlayFootstep()
    {
        if (footstepSource != null && footstepSounds.Length > 0)
        {
            int index = Random.Range(0, footstepSounds.Length);
            footstepSource.clip = footstepSounds[index];
            footstepSource.pitch = Random.Range(0.9f, 1.1f);
            footstepSource.Play();
        }
    }
}