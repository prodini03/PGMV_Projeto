using UnityEngine;
using UnityEngine.UI;

public class DoorInteraction : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    private GameObject jogador;

    [Header("Interaction Settings")]
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private float distanciaMaxima = 3f;
    [SerializeField] private float anguloVisao = 45f;

    [Header("Animation Triggers")]
    [SerializeField] private string abrirTrigger = "Abrir";
    [SerializeField] private string fecharTrigger = "Fechar";

    [Header("Audio Clips")]
    [SerializeField] private AudioClip abrirSom;
    [SerializeField] private AudioClip fecharSom;
    private AudioSource audioSource;


    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player");

        if (jogador == null)
        {
            Debug.LogWarning("Jogador não encontrado! Certifique-se de que a tag 'Player' foi atribuída ao jogador.");
        }

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            // Optional: auto-add one if not present
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (jogador == null || animator == null) return;

        Transform jogadorTransform = jogador.transform;
        float distancia = Vector3.Distance(transform.position, jogadorTransform.position);
        Vector3 direcaoParaPorta = transform.position - jogadorTransform.position;
        float angulo = Vector3.Angle(jogadorTransform.forward, direcaoParaPorta.normalized);

        if (distancia <= distanciaMaxima && angulo <= anguloVisao)
        {

            if (Input.GetKeyDown(interactionKey))
            {
                if (!isOpen)
                {
                    animator.SetTrigger(abrirTrigger);
                    if (abrirSom != null) audioSource.PlayOneShot(abrirSom);
                    isOpen = true;
                }
                else
                {
                    animator.SetTrigger(fecharTrigger);
                    if (fecharSom != null) audioSource.PlayOneShot(fecharSom);
                    isOpen = false;
                }
            }
        }
    }
}
