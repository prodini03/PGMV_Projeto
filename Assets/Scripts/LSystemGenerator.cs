using System.Collections.Generic;
using UnityEngine;

public class LSystemGenerator : MonoBehaviour
{
    public string axiom;
    public int iterations;
    public List<LSystemRule> rules;
    public bool isStochastic = true;

    public string Generate()
    {
        string current = axiom;
        for (int i = 0; i < iterations; i++)
        {
            current = ApplyRules(current);
        }
        return current;
    }

    public string ApplyRules(string input)
    {
        System.Text.StringBuilder sb = new();
        foreach (char c in input)
        {
            string replacement = c.ToString();

            foreach (var rule in rules)
            {
                if (rule.predecessor == c.ToString())
                {
                    if (isStochastic)
                    {
                        float r = Random.value;
                        float cumulative = 0f;
                        for (int i = 0; i < rule.successors.Count; i++)
                        {
                            cumulative += rule.probabilities[i];
                            if (r <= cumulative)
                            {
                                replacement = rule.successors[i];
                                break;
                            }
                        }
                    }
                    else
                    {
                        replacement = rule.successors[0];
                    }
                    break;
                }
            }

            sb.Append(replacement);
        }
        return sb.ToString();
    }
}
