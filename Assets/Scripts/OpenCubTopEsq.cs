using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCubTopEsq : MonoBehaviour
{
  private Animator animator;
    private bool aberta = false;

    private GameObject jogador;
    [SerializeField] private float distanciaMaxima = 3f;
    [SerializeField] private float anguloVisao = 45f;

    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player");
        GameObject textoObject = GameObject.FindWithTag("pressE");

        if (jogador == null)
        {
            Debug.LogWarning("Jogador não encontrado! Certifique-se de que a tag 'Player' foi atribuída ao jogador.");
        }

        animator = GetComponent<Animator>();


    }

    void Update()
    {
        if (jogador == null)
        {
            return;
        }

        Transform jogadorTransform = jogador.transform;
        float distancia = Vector3.Distance(transform.position, jogadorTransform.position);
        Vector3 direcaoParaPorta = transform.position - jogadorTransform.position;
        float angulo = Vector3.Angle(jogadorTransform.forward, direcaoParaPorta.normalized);

        if (distancia <= distanciaMaxima && angulo <= anguloVisao)
        {


            if (Input.GetKeyDown(KeyCode.C))
            {
                if (!aberta)
                {
                    animator.SetTrigger("Abrir");
                    aberta = true;
                }
                else
                {
                    animator.SetTrigger("Fechar");
                    aberta = false;
                }
            }
        }
    

        
    }
}
