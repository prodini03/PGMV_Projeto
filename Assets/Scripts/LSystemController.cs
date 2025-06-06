using System.Collections.Generic;
using UnityEngine;

public class LSystemController : MonoBehaviour
{
    public enum PlantType { Bamboo, FloweringBush }
    public PlantType selectedPlant = PlantType.Bamboo;
    public int iterations = 4;
    public float angle = 25f;
    public float length = 0.3f;

    public List<float> bambooProbabilities = new List<float> { 0.3f, 0.4f, 0.1f, 0.2f };
    public List<float> bushProbabilities = new List<float> { 0.25f, 0.25f, 0.1f, 0.4f };

    private int currentIteration = 0;
    private LSystemGenerator generator;
    private Turtle3DInterpreter interpreter;

    void Awake()
    {
        generator = gameObject.AddComponent<LSystemGenerator>();
        interpreter = gameObject.AddComponent<Turtle3DInterpreter>();
    }

    public void InitializePlant()
    {
        // Limpa a planta
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        // Instancia o vaso
        GameObject vasoPrefab = Resources.Load<GameObject>("Vaso");
        if (vasoPrefab != null)
        {
            GameObject vaso = Instantiate(vasoPrefab, transform.position, Quaternion.identity, transform);
            vaso.name = "Vaso(Clone)";
        }

        // Apenas o caule (axioma)
        generator.axiom = "F";
        generator.iterations = 1;
        generator.isStochastic = true;
        generator.rules = new List<LSystemRule>();

        if (selectedPlant == PlantType.Bamboo)
        {
            generator.rules.Add(new LSystemRule
            {
                predecessor = "F",
                successors = new List<string> { "F[+F\\\\\\L]FL", "//F[////L][-F]", "F[+F[\\\\\\L][L]][-F[//////L][L]]", "F" },
                probabilities = bambooProbabilities
            });
        }
        else
        {
            generator.rules.Add(new LSystemRule
            {
                predecessor = "F",
                successors = new List<string> { "F[^^L][-F\\\\\\X]", "F[^^L][+F///X]", "F[+F\\\\L][\\\\++F\\\\L][///--F//L][-F\\L]FX", "F[^^L][\\+FX&L][/-FX&L]" },
                probabilities = bushProbabilities
            });
        }

        string sequence = generator.Generate();
        interpreter.branchPrefab = Resources.Load<GameObject>("Caule");
        interpreter.leafPrefab = Resources.Load<GameObject>("Folha");
        interpreter.flowerPrefab = Resources.Load<GameObject>("Flor");
        interpreter.angle = angle;
        interpreter.length = length;
        interpreter.branchThickness = (selectedPlant == PlantType.FloweringBush) ? 0.05f : 0.1f;

        // Só o caule, sem folhas nem flores
        interpreter.Interpret(sequence, includeFlowers: false);

        currentIteration = 0;
    }

public void FixBoxColliderToPlantSize()
{
    BoxCollider box = GetComponent<BoxCollider>();
    if (box == null)
        box = gameObject.AddComponent<BoxCollider>();

    Renderer[] renderers = GetComponentsInChildren<Renderer>();
    if (renderers.Length == 0)
        return;

    Bounds bounds = renderers[0].bounds;
    foreach (Renderer rend in renderers)
        bounds.Encapsulate(rend.bounds);

    Vector3 localCenter = transform.InverseTransformPoint(bounds.center);
    Vector3 localSize = transform.InverseTransformVector(bounds.size);

    float minSizeXZ = 0.3f;

    localSize.x = Mathf.Max(localSize.x, minSizeXZ);
    localSize.z = Mathf.Max(localSize.z, minSizeXZ);

    box.center = localCenter;
    box.size = localSize;
}



    public void GrowNextIteration()
    {
        if (currentIteration >= iterations) return;

        currentIteration++;

        foreach (Transform child in transform)
        {
            if (child.name != "Vaso(Clone)")
                Destroy(child.gameObject);
        }

        generator.iterations = currentIteration;
        string sequence = generator.Generate();

        bool isFinal = currentIteration == iterations;

        interpreter.Interpret(sequence, includeFlowers: isFinal);
        FixBoxColliderToPlantSize();
    }

    public void ResetPlant()
    {
        InitializePlant();
    }
}
