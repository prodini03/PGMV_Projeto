using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public float iterationInterval = 2f;

    private List<LSystemController> allPlants = new();
    private Coroutine growthCoroutine;
    private bool isPaused = false;

    IEnumerator  Start()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject[] pickupObjects = GameObject.FindGameObjectsWithTag("Pickup");
        foreach (var obj in pickupObjects)
        {
            var controller = obj.GetComponent<LSystemController>();
            if (controller != null && !allPlants.Contains(controller))
            {
                allPlants.Add(controller);
                controller.InitializePlant();
            }
        }
    }

    public void Play()
    {
        isPaused = false;
        if (growthCoroutine == null)
            growthCoroutine = StartCoroutine(GrowLoop());
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Restart()
    {
        if (growthCoroutine != null)
        {
            StopCoroutine(growthCoroutine);
            growthCoroutine = null;
        }

        foreach (var plant in allPlants)
            plant.ResetPlant();

    }

    IEnumerator GrowLoop()
    {
        while (true)
        {
            if (!isPaused)
            {
                foreach (var plant in allPlants)
                    plant.GrowNextIteration();
            }

            yield return new WaitForSeconds(iterationInterval);
        }
    }
}