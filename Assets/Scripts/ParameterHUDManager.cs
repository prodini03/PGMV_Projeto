using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParameterHUDManager : MonoBehaviour
{
    public GameObject hudPanel;

    public Slider iterationsSlider;
    public TMP_Text iterationsValueText;

    public Slider angleSlider;
    public TMP_Text angleValueText;

    public Slider lengthSlider;
    public TMP_Text lengthValueText;

    public Toggle windToggle;

    private PlantState currentPlant;

    private PlantState lastPlant;


    void Start()
    {
        // Remove listeners antigos para evitar duplicados
        iterationsSlider.onValueChanged.RemoveAllListeners();
        angleSlider.onValueChanged.RemoveAllListeners();
        lengthSlider.onValueChanged.RemoveAllListeners();

        // Adiciona listeners por código
        iterationsSlider.onValueChanged.AddListener(OnIterationsSliderChanged);
        angleSlider.onValueChanged.AddListener(OnAngleSliderChanged);
        lengthSlider.onValueChanged.AddListener(OnLengthSliderChanged);
    }

    void Update()
    {
        var plants = FindObjectsOfType<PlantState>();
        currentPlant = plants.FirstOrDefault(p => p.isBeingHeld);

        bool holdingPlant = currentPlant != null && currentPlant.isBeingHeld;
        hudPanel.SetActive(holdingPlant);

        // Só atualiza sliders se for uma nova planta agarrada
        if (holdingPlant && currentPlant != lastPlant)
        {
            LoadPlantParameters();
            lastPlant = currentPlant;
        }

        if (!holdingPlant)
        {
            lastPlant = null;
        }
    }


    void LoadPlantParameters()
    {
        var controller = currentPlant.GetComponent<LSystemController>();

        iterationsSlider.SetValueWithoutNotify(controller.iterations);
        angleSlider.SetValueWithoutNotify(controller.angle);
        lengthSlider.SetValueWithoutNotify(controller.length);

        iterationsValueText.text = controller.iterations.ToString();
        angleValueText.text = controller.angle.ToString("0.0");
        lengthValueText.text = controller.length.ToString("0.00");
    }

    public void OnIterationsSliderChanged(float value)
    {
        iterationsValueText.text = Mathf.RoundToInt(value).ToString();
        if (currentPlant != null)
            currentPlant.GetComponent<LSystemController>().iterations = Mathf.RoundToInt(value);
        print(currentPlant.GetComponent<LSystemController>().iterations);
    }

    public void OnAngleSliderChanged(float value)
    {
        angleValueText.text = value.ToString("0.0");
        if (currentPlant != null)
            currentPlant.GetComponent<LSystemController>().angle = value;
        print(currentPlant.GetComponent<LSystemController>().angle);
    }

    public void OnLengthSliderChanged(float value)
    {
        lengthValueText.text = value.ToString("0.00");
        if (currentPlant != null)
            currentPlant.GetComponent<LSystemController>().length = value;
            print(currentPlant.GetComponent<LSystemController>().length);
    }
}
