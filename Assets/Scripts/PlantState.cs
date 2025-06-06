using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantState : MonoBehaviour
{
    public bool isBeingHeld = false;
    public bool isStored = false;
    public void SetStored(bool stored)
    {
        isStored = stored;

        foreach (var wind in GetComponentsInChildren<WindController>())
        {
            wind.ToggleWind(!stored);
        }
    }
}
