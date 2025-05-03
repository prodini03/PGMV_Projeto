using UnityEngine;

public class DoorTriggerInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private AudioClip abrirSom;
    [SerializeField] private AudioClip fecharSom;
    [SerializeField] private KeyCode interactionKey = KeyCode.X;

    private AudioSource audioSource;
    private bool isOpen = false;
    private bool playerInZone = false;

    [Header("Animation Triggers")]
    [SerializeField] private string abrirTrigger = "Abrir";
    [SerializeField] private string fecharTrigger = "Fechar";

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        Debug.Log("start");
    }

    void Update()
    {
        
        if (playerInZone && Input.GetKeyDown(interactionKey))
        {
            Debug.Log("Em zona de abrir porta");
            if (!isOpen)
            {
                doorAnimator.SetTrigger(abrirTrigger);
                if (abrirSom != null) audioSource.PlayOneShot(abrirSom);
            }
            else
            {
                doorAnimator.SetTrigger(fecharTrigger);
                if (fecharSom != null) audioSource.PlayOneShot(fecharSom);
            }

            isOpen = !isOpen;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Braco"))
        {
            Debug.Log("Player entered trigger");
            playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Braco"))
        {
            playerInZone = false;
        }
    }
}
