using System.Collections.Generic;

[System.Serializable]
public class LSystemRule
{
    public string predecessor;                  // Ex: "F"
    public List<string> successors;             // Ex: ["F[+F]F[-F]F", "F[+F]F"]
    public List<float> probabilities;           // Ex: [0.5f, 0.5f]
}
