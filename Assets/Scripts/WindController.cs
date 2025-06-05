using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
 public float windStrength = 10f;
    public float windSpeed = 5f;
    public Vector3 rotationAxis = Vector3.forward;

    public bool windEnabled = true;

    private Quaternion initialRotation;
    private float offset;

    void Start()
    {
        initialRotation = transform.localRotation;
        offset = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (!windEnabled)
        {
            transform.localRotation = initialRotation;
            return;
        }

        float angle = Mathf.Sin(Time.time * windSpeed + offset) * windStrength;
        transform.localRotation = initialRotation * Quaternion.AngleAxis(angle, rotationAxis);
    }

    public void ToggleWind(bool enabled)
    {
        windEnabled = enabled;
    }
}
