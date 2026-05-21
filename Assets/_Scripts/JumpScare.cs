using UnityEngine;

public class JumpScare : MonoBehaviour
{
    public AudioSource soundToPlay; // El sonido del susto
    public GameObject objectToShow; // Opcional: Un monstruo o imagen que aparece de golpe

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que tu Player tenga el Tag "Player"
        {
            if (soundToPlay != null)
            {
                soundToPlay.Play();
            }

            if (objectToShow != null)
            {
                objectToShow.SetActive(true);
                // Truco: Desactívalo después de 1 segundo para que sea un flashazo
                Destroy(objectToShow, 1.5f);
            }

            Debug.Log("¡Susto activado!");
            // Destruimos el trigger para que no vuelva a pasar
            Destroy(gameObject, 4f);
        }
    }
}