using System.Collections.Generic;
using UnityEngine;

public class LSystemController : MonoBehaviour
{
    public enum PlantType { Tree, FloweringBush }
    public PlantType selectedPlant = PlantType.Tree;

    public int iterations = 4;
    public float angle = 25f;
    public float length = 1f;

    void Start()
    {
        var generator = gameObject.AddComponent<LSystemGenerator>();
        generator.axiom = "F";
        generator.iterations = iterations;
        generator.isStochastic = true;
        generator.rules = new List<LSystemRule>();

        if (selectedPlant == PlantType.Tree)
        {
            generator.rules.Add(new LSystemRule
            {
                predecessor = "F",
                successors = new List<string> { "F[+F]F[-F]F", "F[+F]F", "F[-F]F" },
                probabilities = new List<float> { 0f, 1f, 0f }
            });
        }
        else if (selectedPlant == PlantType.FloweringBush)
        {
            generator.rules.Add(new LSystemRule
            {
                predecessor = "F",
                successors = new List<string> { "F[+F[L]]F[-F[X]]", "F[+F]F", "F[-F]F" },
                probabilities = new List<float> { 0.5f, 0.25f, 0.25f }
            });
        }

        string result = generator.Generate();

        var interpreter = gameObject.AddComponent<Turtle3DInterpreter>();
        interpreter.branchPrefab = Resources.Load<GameObject>("Caule");
        interpreter.leafPrefab = Resources.Load<GameObject>("Folha");
        interpreter.flowerPrefab = Resources.Load<GameObject>("Flor");
        interpreter.angle = angle;
        interpreter.length = length;

        interpreter.Interpret(result);
    }
}