using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantKeepyUp : MonoBehaviour
{
    public float kickForce = 5f;
    public string plantTag = "Pickup";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(plantTag))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 kickDirection = Vector3.up + transform.forward * 0.2f;
                rb.AddForce(kickDirection.normalized * kickForce, ForceMode.Impulse);
            }
        }
    }
}
