using Unity.VisualScripting;
using UnityEngine;

public class DoorTriggerInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private AudioClip abrirSom;
    [SerializeField] private AudioClip fecharSom;
    [SerializeField] private KeyCode interactionKey = KeyCode.X;

    private AudioSource audioSource;
    private string componentName;
    private bool isOpen = false;
    private bool playerInZone = false;

    private DoorsState doorsState;

    [Header("Animation Triggers")]
    [SerializeField] private string abrirTrigger = "Abrir";
    [SerializeField] private string fecharTrigger = "Fechar";

    void Start()
    {
        getModuleName();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        Transform current = transform;
        while (current != null)
        {
            doorsState = current.GetComponent<DoorsState>();
            if (doorsState != null)
                break;
            current = current.parent;
        }

        if (doorsState == null)
            Debug.LogWarning("DoorsState não encontrado!");
        print(componentName);
    }

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(interactionKey))
        {
            if (!isOpen)
            {
                if (componentName == "portadireita")
                {
                    if(doorsState.insideDoorsOpen == 0) 
                    {
                        doorsState.rightDoorOpen = true;
                        doorAnimator.SetTrigger(abrirTrigger);
                        if (abrirSom != null) audioSource.PlayOneShot(abrirSom);
                        isOpen = !isOpen;
                    }
                }
                else if (componentName == "portaesquerda" && doorsState.insideDoorsOpen == 0) 
                {
                    if(doorsState.insideDoorsOpen == 0)
                    {
                        doorAnimator.SetTrigger(abrirTrigger);
                        if (abrirSom != null) audioSource.PlayOneShot(abrirSom);
                        doorsState.leftDoorOpen = true;
                        isOpen = !isOpen;
                    }
                }
                else
                {
                    if(componentName.StartsWith("Modulo_R"))
                    {
                        if(doorsState.rightDoorOpen == true && doorsState.leftDoorOpen == false)
                        {
                            doorAnimator.SetTrigger(abrirTrigger);
                            if (abrirSom != null) audioSource.PlayOneShot(abrirSom);
                            doorsState.insideDoorsOpen++;
                            isOpen = !isOpen;
                        }
                    }
                    else if (componentName.StartsWith("Modulo_L"))
                    {
                        if(doorsState.rightDoorOpen == false && doorsState.leftDoorOpen == true)
                        {
                            doorAnimator.SetTrigger(abrirTrigger);
                            if (abrirSom != null) audioSource.PlayOneShot(abrirSom);
                            doorsState.insideDoorsOpen++;
                            isOpen = !isOpen;
                        }
                    }
                }
            }
            else
            {
                if (componentName == "portadireita")
                {
                    if (doorsState.insideDoorsOpen == 0)
                    {
                        doorsState.rightDoorOpen = false;
                        doorAnimator.SetTrigger(fecharTrigger);
                        if (fecharSom != null) audioSource.PlayOneShot(fecharSom);
                        isOpen = !isOpen;
                    }
                }
                else if (componentName == "portaesquerda")
                {
                    if(doorsState.insideDoorsOpen == 0)
                    {
                        doorsState.leftDoorOpen = false;
                        doorAnimator.SetTrigger(fecharTrigger);
                        if (fecharSom != null) audioSource.PlayOneShot(fecharSom);
                        isOpen = !isOpen;
                    }
                }
                else
                {
                    if (componentName.StartsWith("Modulo_R"))
                    {
                        doorAnimator.SetTrigger(fecharTrigger);
                        if (fecharSom != null) audioSource.PlayOneShot(fecharSom);
                        doorsState.insideDoorsOpen--;
                        isOpen = !isOpen;
                    }
                    else if(componentName.StartsWith("Modulo_L"))
                    {
                        doorAnimator.SetTrigger(fecharTrigger);
                        if (fecharSom != null) audioSource.PlayOneShot(fecharSom);
                        doorsState.insideDoorsOpen--;
                        isOpen = !isOpen;
                    }
                }
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Braco"))
        {
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

    private void getModuleName()
    {
        GameObject obj = gameObject;

        while (obj != null && !(obj.name.StartsWith("Modulo") || obj.name.StartsWith("porta")))
        {
            obj = obj.transform.parent?.gameObject;
        }
        if (obj != null)
        {
            componentName = obj.name;
        }
        else
        {
            Debug.LogWarning("No parent named 'Modulo' found.");
        }
        
    }
}
