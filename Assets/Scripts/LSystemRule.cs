using System.Collections.Generic;

[System.Serializable]
public class LSystemRule
{
    public string predecessor;
    public List<string> successors;
    public List<float> probabilities;
}
