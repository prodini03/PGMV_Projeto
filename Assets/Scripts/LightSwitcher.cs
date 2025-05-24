using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class LightSwitcher : MonoBehaviour
{

    [SerializeField] private KeyCode interactionKey = KeyCode.L;
    [SerializeField] private Renderer targetRenderer; 
    [SerializeField] private Material lightOnMaterial;
    [SerializeField] private Material lightOffMaterial;
    [SerializeField] private AudioClip lightSwitch;

    private AudioSource audioSource;
    private bool lightOn = true;
    private bool playerInZone = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.1f;
    }

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(interactionKey))
        {
            if (!lightOn)
            {
                if (lightSwitch != null) audioSource.PlayOneShot(lightSwitch);
                StartCoroutine(switchLigths(true));
            }
            else
            {
                if (lightSwitch != null) audioSource.PlayOneShot(lightSwitch);
                StartCoroutine(switchLigths(false));
            }

            lightOn = !lightOn;
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

    private IEnumerator switchLigths(Boolean on)
    {
        yield return new WaitForSeconds(0.25f);
        foreach (Light light in GetComponentsInChildren<Light>())
        {
            light.enabled = on;
            if(on) targetRenderer.material = lightOnMaterial;
            else targetRenderer.material = lightOffMaterial;
        }
    }
}
