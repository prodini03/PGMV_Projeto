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
        heldObject.transform.SetParent(transform, true);
        heldObject.transform.localScale = new Vector3(2.33f, 1.33f, 1.33f);
        heldObject.transform.localRotation = Quaternion.AngleAxis(0, new Vector3(1, 1, 1));
        heldObject.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));
        heldObject.transform.localPosition = new Vector3(0.4f, -0.3f, -0.1f);
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
        heldObject.GetComponent<Collider>().enabled = false;
    }


    void Drop()
    {
        heldObject.transform.SetParent(null);
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.GetComponent<Collider>().enabled = true;
        rb.AddForce(transform.forward * 0.01f, ForceMode.Impulse);
        rb.transform.localScale = new Vector3(5,5,5);
        rb.transform.localRotation = Quaternion.AngleAxis(0, new Vector3(1, 1, 1));
        heldObject = null;
    }
}
