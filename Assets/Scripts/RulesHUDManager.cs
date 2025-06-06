using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RulesHUDManager : MonoBehaviour
{
    public GameObject hudPanel;

    public Slider[] ruleProbabilitySliders;
    public TMP_Text[] ruleValueTexts;
    public TMP_Text[] ruleDescriptionTexts; 
    public TMP_Text probabilityTotalText; 

    private PlantState currentPlant;
    private PlantState lastPlant;

    private readonly string[] bambooDescriptions = {
        "F[+F\\\\\\L]FL",
        "//F[////L][-F]",
        "F[+F[\\\\\\L][L]][-F[//////L][L]]",
        "F"
    };
    private readonly string[] bushDescriptions = {
        "F[^^L][-F\\\\\\X]",
        "F[^^L][+F///X]",
        "F[+F\\\\L][\\\\++F\\\\L][///--F//L][-F\\L]FX",
        "F[^^L][\\+FX&L][/-FX&L]"
    };

    void Start()
    {
        for (int i = 0; i < ruleProbabilitySliders.Length; i++)
        {
            int idx = i;
            ruleProbabilitySliders[i].onValueChanged.RemoveAllListeners();
            ruleProbabilitySliders[i].onValueChanged.AddListener((value) => OnProbabilitySliderChanged(idx, value));
        }
    }

    void Update()
    {
        var plants = FindObjectsOfType<PlantState>();
        currentPlant = plants.FirstOrDefault(p => p.isBeingHeld);

        bool holdingPlant = currentPlant != null && currentPlant.isBeingHeld;
        hudPanel.SetActive(holdingPlant);

        if (holdingPlant && currentPlant != lastPlant)
        {
            LoadProbabilitiesAndDescriptions();
            lastPlant = currentPlant;
        }

        if (!holdingPlant)
        {
            lastPlant = null;
        }
    }

    void LoadProbabilitiesAndDescriptions()
    {
        if (currentPlant == null) return;
        var controller = currentPlant.GetComponent<LSystemController>();
        List<float> probs;
        string[] descriptions;

        if (controller.selectedPlant == LSystemController.PlantType.Bamboo)
        {
            probs = controller.bambooProbabilities;
            descriptions = bambooDescriptions;
        }
        else
        {
            probs = controller.bushProbabilities;
            descriptions = bushDescriptions;
        }

        float total = 0f;
        for (int i = 0; i < ruleProbabilitySliders.Length; i++)
        {
            float prob = (i < probs.Count) ? probs[i] : 0f;
            ruleProbabilitySliders[i].SetValueWithoutNotify(prob);
            ruleValueTexts[i].text = prob.ToString("0.00");
            ruleDescriptionTexts[i].text = descriptions[i];
            total += prob;
        }
        probabilityTotalText.text = "Total: " + total.ToString("0.00");
    }

    void OnProbabilitySliderChanged(int index, float value)
    {
        if (currentPlant == null) return;
        var controller = currentPlant.GetComponent<LSystemController>();
        List<float> probs = controller.selectedPlant == LSystemController.PlantType.Bamboo
            ? controller.bambooProbabilities
            : controller.bushProbabilities;

        probs[index] = value;

        float total = probs.Sum();
        if (total > 1f)
        {
            for (int i = 0; i < probs.Count; i++)
            {
                probs[i] /= total;
                ruleProbabilitySliders[i].SetValueWithoutNotify(probs[i]);
                ruleValueTexts[i].text = probs[i].ToString("0.00");
            }
            total = 1f;
        }
        else
        {
            ruleValueTexts[index].text = value.ToString("0.00");
        }

        probabilityTotalText.text = "Total: " + total.ToString("0.00");
    }
}