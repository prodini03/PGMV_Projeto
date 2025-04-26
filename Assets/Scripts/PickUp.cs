using UnityEngine;

public class ArmPickup : MonoBehaviour
{
    private GameObject objectInReach;
    private GameObject heldObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (heldObject == null && objectInReach != null)
            {
                PickUp(objectInReach);
            }
            else if (heldObject != null)
            {
                Drop();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            objectInReach = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            if (objectInReach == other.gameObject)
                objectInReach = null;
        }
    }

void PickUp(GameObject obj)
{
    heldObject = obj;
    heldObject.transform.SetParent(transform);
    heldObject.transform.localPosition = new Vector3(0f, 0f, 0f);
    heldObject.GetComponent<Rigidbody>().isKinematic = true;
}


    void Drop()
    {
        heldObject.transform.SetParent(null);
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(transform.forward * 2f, ForceMode.Impulse);
        heldObject = null;
    }
}
