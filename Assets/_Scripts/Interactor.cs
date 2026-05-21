using UnityEngine;
using TMPro; // Necesario para el texto

public class Interactor : MonoBehaviour
{
    public float interactDistance = 3f;
    public LayerMask interactLayer; // Aquí pondrás la capa "Interactable"
    public TextMeshProUGUI interactText; // Aquí pondrás el texto "Pulsa E"

    void Update()
    {
        RaycastHit hit;
        // Lanza rayo desde la posición y dirección de la cámara
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance, interactLayer))
        {
            if (hit.collider.CompareTag("Interactable")) // Recuerda poner el Tag al objeto
            {
                if (interactText != null)
                {
                    interactText.text = "Presiona E";
                    interactText.gameObject.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Busca el script del objeto y ejecútalo
                    var obj = hit.collider.GetComponent<InteractableObject>();
                    if (obj != null) obj.DoInteraction();
                }
            }
        }
        else
        {
            if (interactText != null) interactText.gameObject.SetActive(false);
        }
    }
}