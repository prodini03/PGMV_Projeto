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



    void Start()
    {
        iterationsSlider.onValueChanged.RemoveAllListeners();
        angleSlider.onValueChanged.RemoveAllListeners();
        lengthSlider.onValueChanged.RemoveAllListeners();
        windToggle.onValueChanged.RemoveAllListeners();

        iterationsSlider.onValueChanged.AddListener(OnIterationsSliderChanged);
        angleSlider.onValueChanged.AddListener(OnAngleSliderChanged);
        lengthSlider.onValueChanged.AddListener(OnLengthSliderChanged);
        windToggle.onValueChanged.AddListener(OnWindToggleChanged);
    }

    void Update()
    {
        var plants = FindObjectsOfType<PlantState>();
        currentPlant = plants.FirstOrDefault(p => p.isBeingHeld);

        bool holdingPlant = currentPlant != null && currentPlant.isBeingHeld;
        hudPanel.SetActive(holdingPlant);

        if (holdingPlant)
        {
            LoadPlantParameters();
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
    }

    public void OnAngleSliderChanged(float value)
    {
        angleValueText.text = value.ToString("0.0");
        if (currentPlant != null)
            currentPlant.GetComponent<LSystemController>().angle = value;
    }

    public void OnLengthSliderChanged(float value)
    {
        lengthValueText.text = value.ToString("0.00");
        if (currentPlant != null)
            currentPlant.GetComponent<LSystemController>().length = value;
    }

    void OnWindToggleChanged(bool isOn)
    {
        if (currentPlant == null) return;

        WindController[] windControllers = currentPlant.GetComponentsInChildren<WindController>();

        foreach (var controller in windControllers)
        {
            controller.ToggleWind(isOn);
        }
    }
}