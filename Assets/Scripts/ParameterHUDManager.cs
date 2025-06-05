using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParameterHUDManager : MonoBehaviour
{
    public GameObject hudPanel;
    public Slider iterationsSlider;
    public Slider angleSlider;
    public Slider lengthSlider;
    public Toggle windToggle;
    private PlantState currentPlant;

    void Update()
{
    if (currentPlant == null || !currentPlant.isBeingHeld)
    {
        var plants = FindObjectsOfType<PlantState>();
        currentPlant = plants.FirstOrDefault(p => p.isBeingHeld);
    }

    hudPanel.SetActive(currentPlant != null && currentPlant.isBeingHeld);

    if (currentPlant != null && currentPlant.isBeingHeld)
    {
        LoadPlantParameters();
    }
}


    void LoadPlantParameters()
    {
        var controller = currentPlant.GetComponent<LSystemController>();

        iterationsSlider.value = controller.iterations;
        angleSlider.value = controller.angle;
        lengthSlider.value = controller.length;
    }
}
