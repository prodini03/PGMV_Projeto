// LSystemController.cs
// Este script gera uma planta automaticamente, incluindo um vaso na base.
// Ele carrega os prefabs de "Caule", "Folha", "Flor" e "Vaso" da pasta Resources e cria dois tipos diferentes de planta: Árvore ou Arbusto com Flores.

using System.Collections.Generic;
using UnityEngine;

public class LSystemController : MonoBehaviour
{
    public enum PlantType { Bamboo, FloweringBush } // Tipos de planta
    public PlantType selectedPlant = PlantType.Bamboo; // Escolher no Inspector

    public int iterations = 4; // Número de iterações
    public float angle = 25f;
    public float length = 0.3f;

    void Start()
    {
        // 1. Instancia o vaso na base da planta
        GameObject vasoPrefab = Resources.Load<GameObject>("Vaso");
        if (vasoPrefab != null)
        {
            Instantiate(vasoPrefab, transform.position, Quaternion.identity, this.transform);
        }
        else
        {
            Debug.LogWarning("Prefab 'Vaso' não encontrado na pasta Resources.");
        }

        // 2. Cria e configura o gerador
        LSystemGenerator generator = gameObject.AddComponent<LSystemGenerator>();
        generator.axiom = "F";
        generator.iterations = iterations;
        generator.isStochastic = true;
        generator.rules = new List<LSystemRule>();

        // 3. Define regras diferentes conforme o tipo de planta
        if (selectedPlant == PlantType.Bamboo) // Mudar para Bamboo
        {
            generator.rules.Add(new LSystemRule
            {
                predecessor = "F",
                successors = new List<string> { "F[+F\\\\\\L]FL", "//F[////L][-F]", "F[+F[\\\\\\L][L]][-F[//////L][L]]" },
                probabilities = new List<float> { 0.3f, 0.4f, 0.1f, 0.2f }
            });
        }
        else if (selectedPlant == PlantType.FloweringBush)  // Este ficava tipo um arbusto com flores
        {
            generator.rules.Add(new LSystemRule
            {
                predecessor = "F",
                successors = new List<string> { "F[^^L][-F\\\\\\X]", "F[^^L][+F///X]", "F[+F\\\\L][\\\\++F\\\\L][///--F//L][-F\\L]FX","F[^^L][\\+FX&L][/-FX&L]" },
                probabilities = new List<float> { 0.25f, 0.25f, 0.1f , 0.4f }
            });
        }

        // 4. Gera a string final da planta
        string sequence = generator.Generate();

        // 5. Cria e configura o interpretador
        Turtle3DInterpreter interpreter = gameObject.AddComponent<Turtle3DInterpreter>();
        interpreter.branchPrefab = Resources.Load<GameObject>("Caule");
        interpreter.leafPrefab = Resources.Load<GameObject>("Folha");
        interpreter.flowerPrefab = Resources.Load<GameObject>("Flor");
        interpreter.angle = angle;
        interpreter.length = length;
        if (selectedPlant == PlantType.FloweringBush)
            interpreter.branchThickness = 0.05f; // Mais fino para o arbusto
        else
            interpreter.branchThickness = 0.1f;  // Valor normal para outros

        // 6. Interpreta a sequência e instancia a planta
        interpreter.Interpret(sequence);
    }
}
