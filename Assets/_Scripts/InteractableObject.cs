using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // Añadimos 'Note' para que el rayo de la cámara sepa que es una nota y ponga la mano
    public enum InteractionType { Key, Door, Note } 
    public InteractionType type;

    [Header("Sonido")]
    public AudioClip sonidoInteraccion;
    public float volumen = 1f;

    private GameManager gameManager;

    void Start()
    {
        gameManager = Object.FindObjectOfType<GameManager>();
    }

    public void DoInteraction()
    {
        if (gameManager == null) return;

        if (sonidoInteraccion != null)
        {
            AudioSource.PlayClipAtPoint(sonidoInteraccion, transform.position, volumen);
        }

        if (type == InteractionType.Key)
        {
            gameManager.AddKey();
            Destroy(gameObject);
        }
        else if (type == InteractionType.Door)
        {
            if (gameManager.HasAllKeys())
            {
                gameManager.WinGame();
            }
            else
            {
                Debug.Log("No puedes escapar. Te faltan llaves.");
            }
        }
        else if (type == InteractionType.Note) // Lógica para la carta de Amy
        {
            // Busca el script NotaInteractuable en este mismo objeto
            NotaInteractuable scriptNota = GetComponent<NotaInteractuable>();
            if (scriptNota != null)
            {
                scriptNota.AbrirDesdeMano();
            }
        }
    }
}