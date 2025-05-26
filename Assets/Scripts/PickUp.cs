using UnityEngine;

public class ArmPickup : MonoBehaviour
{

    [SerializeField] private float detectionRadius = 0.5f;
    [SerializeField] private LayerMask compartimentoLayer;

    private GameObject objectInReach;
    private GameObject heldObject;
    private GameObject compartimento;

    private bool isCompartimento = false;
    private bool playerInZone = false;

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
        else if (other.CompareTag("Compartimento"))
        {
            if (objectInReach == other.gameObject)
            {
                objectInReach = null;
                isCompartimento = true;
                compartimento = other.gameObject;
            }

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
        heldObject.transform.localRotation = Quaternion.AngleAxis(80, new Vector3(1, 0, 0));
        heldObject.transform.localPosition = new Vector3(0.4f, -0.3f, -0.07f);
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
        heldObject.GetComponent<Collider>().enabled = false;
    }

    void Drop()
    {
        Vector3 dropPos = transform.position + transform.forward * 0.5f;

        Collider[] colliders = Physics.OverlapSphere(dropPos, detectionRadius, compartimentoLayer);

        if (colliders.Length > 0)
        {
            Transform compartimento = colliders[0].transform;

            heldObject.transform.SetParent(compartimento);
            heldObject.transform.localPosition = Vector3.zero;
            heldObject.transform.localRotation = Quaternion.identity;
            if (compartimento.gameObject.name.StartsWith("Gaveta")) 
            {
                heldObject.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));
                heldObject.transform.localPosition = new Vector3(-0.15f, 0.3f, -0.37f);
            }
            else if (compartimento.gameObject.name.StartsWith("Cubiculo"))
            {
                if(compartimento.gameObject.name == "CubiculoTopDir") heldObject.transform.localPosition = new Vector3(-0.391f, 0.045f, -0.453f);
                else if (compartimento.gameObject.name == "CubiculoTopEsq") heldObject.transform.localPosition = new Vector3(-0.571f, 0.056f, 0.469f);
                else if(compartimento.gameObject.name == "CubiculoBotEsq") heldObject.transform.localPosition = new Vector3(-0.509f, -0.871f, 0.449f);
                else if (compartimento.gameObject.name == "CubiculoBotDir") heldObject.transform.localPosition = new Vector3(-0.498f, -0.905f, -0.512f);
            }
            if(compartimento.gameObject.name.StartsWith("Prateleira"))
            {
                heldObject.transform.localPosition = new Vector3(-0.0039f, -0.0026f, 0.0002f);
            }
                
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            heldObject.GetComponent<Collider>().enabled = false;

            Debug.Log("Planta largada dentro de compartimento");

            heldObject = null;
        }
        else
        {
            heldObject.transform.SetParent(null);

            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            heldObject.GetComponent<Collider>().enabled = true;

            rb.AddForce(transform.forward * 0.01f, ForceMode.Impulse);
            heldObject.transform.localScale = new Vector3(5, 5, 5);
            heldObject.transform.localRotation = Quaternion.identity;

            Debug.Log("Planta largada fora do compartimento");

            heldObject = null;
        }
    }
}
