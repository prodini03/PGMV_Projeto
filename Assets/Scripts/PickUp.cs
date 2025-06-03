using UnityEngine;

public class ArmPickup : MonoBehaviour
{

    [SerializeField] private float detectionRadius = 0.5f;
    [SerializeField] private LayerMask compartimentoLayer;

    private GameObject objectInReach;
    private GameObject heldObject;
    private GameObject compartimento;
    private CompartimentoState compartimentoState;

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
        Vector3 dropPos = transform.position + transform.forward * 0.5f;
        Collider[] colliders = Physics.OverlapSphere(dropPos, detectionRadius, compartimentoLayer);

        if (colliders.Length > 0)
        {
            Transform compartimento = null;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].name.StartsWith("DoorPlantCollider"))
                    compartimento = colliders[i].transform;
            }
            compartimentoState = getCompartimentoState(compartimento);
            if(compartimentoState != null)
            {
                if (compartimentoState.isOpen)
                {
                    heldObject = obj;
                    heldObject.transform.SetParent(transform, true);
                    heldObject.transform.localScale = new Vector3(2.33f, 1.33f, 1.33f);
                    heldObject.transform.localRotation = Quaternion.AngleAxis(0, new Vector3(1, 1, 1));
                    heldObject.transform.localRotation = Quaternion.AngleAxis(80, new Vector3(1, 0, 0));
                    heldObject.transform.localPosition = new Vector3(0.4f, -0.3f, -0.07f);
                    heldObject.GetComponent<Rigidbody>().isKinematic = true;
                    heldObject.GetComponent<Collider>().enabled = false;
                    heldObject.GetComponent<PlantState>().isBeingHeld = true;
                }
            }
            else
            {
                heldObject = obj;
                heldObject.transform.SetParent(transform, true);
                heldObject.transform.localScale = new Vector3(2.33f, 1.33f, 1.33f);
                heldObject.transform.localRotation = Quaternion.AngleAxis(0, new Vector3(1, 1, 1));
                heldObject.transform.localRotation = Quaternion.AngleAxis(80, new Vector3(1, 0, 0));
                heldObject.transform.localPosition = new Vector3(0.4f, -0.3f, -0.07f);
                heldObject.GetComponent<Rigidbody>().isKinematic = true;
                heldObject.GetComponent<Collider>().enabled = false;
                heldObject.GetComponent<PlantState>().isBeingHeld = true;
            }
        }
        else
        {
            heldObject = obj;
            heldObject.transform.SetParent(transform, true);
            heldObject.transform.localScale = new Vector3(2.33f, 1.33f, 1.33f);
            heldObject.transform.localRotation = Quaternion.AngleAxis(0, new Vector3(1, 1, 1));
            heldObject.transform.localRotation = Quaternion.AngleAxis(80, new Vector3(1, 0, 0));
            heldObject.transform.localPosition = new Vector3(0.4f, -0.3f, -0.07f);
            heldObject.GetComponent<Rigidbody>().isKinematic = true;
            heldObject.GetComponent<Collider>().enabled = false;
            heldObject.GetComponent<PlantState>().isBeingHeld = true;
        }
    }

    void Drop()
    {
        Vector3 dropPos = transform.position + transform.forward * 0.5f;

        Collider[] colliders = Physics.OverlapSphere(dropPos, detectionRadius, compartimentoLayer);


        if (colliders.Length > 0)
        {
            Transform compartimento = null;
            for (int i = 0; i < colliders.Length; i++)
            {
                print(colliders[i].name);
                if (colliders[i].name.StartsWith("DoorPlantCollider"))
                    compartimento = colliders[i].transform;
            }
            print(compartimento.name);
            compartimentoState = getCompartimentoState(compartimento);
            if (compartimentoState != null)
            {
                if (compartimentoState.isOpen)
                {

                    heldObject.transform.SetParent(compartimento);
                    heldObject.transform.localPosition = Vector3.zero;
                    heldObject.transform.localRotation = Quaternion.identity;
                    if (compartimento.name.EndsWith("GavetaBot"))
                    {
                        heldObject.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));
                        heldObject.transform.localPosition = new Vector3(-0.12f, 0.24f, -2.44f);
                    }
                    else if (compartimento.name.EndsWith("GavetaTop"))
                    {
                        heldObject.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));
                        heldObject.transform.localPosition = new Vector3(-0.12f, 0.24f, -0.662f);
                    }

                    else if (compartimento.name.EndsWith("CubiculoTopDir")) heldObject.transform.localPosition = new Vector3(1f, -0.5f, 0f);
                    else if (compartimento.name.EndsWith("CubiculoTopEsq")) heldObject.transform.localPosition = new Vector3(1f, -0.5f, 0f);
                    else if (compartimento.name.EndsWith("CubiculoBotEsq")) heldObject.transform.localPosition = new Vector3(1f, -0.5f, 0f);
                    else if (compartimento.name.EndsWith("CubiculoBotDir")) heldObject.transform.localPosition = new Vector3(1f, -0.5f, 0f);

                    Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                    rb.isKinematic = true;

                    Debug.Log("Planta largada dentro de compartimento");

                    heldObject.GetComponent<Collider>().enabled = false;

                    heldObject.GetComponent<PlantState>().isBeingHeld = false;
                    heldObject = null;
                }
            }
            else
            {
                heldObject.transform.SetParent(compartimento);
                heldObject.transform.localPosition = Vector3.zero;
                heldObject.transform.localRotation = Quaternion.identity;

                if (compartimento.name.EndsWith("PrateleiraBot"))
                {
                    heldObject.transform.localPosition = new Vector3(-0.15f, -0.25f, 0f);
                }
                if (compartimento.name.EndsWith("PrateleiraTop"))
                {
                    heldObject.transform.localPosition = new Vector3(-0.15f, -0.25f, 0f);
                }
                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                heldObject.GetComponent<Collider>().enabled = false;

                heldObject.GetComponent<PlantState>().isBeingHeld = false;

                Debug.Log("Planta largada dentro de compartimento");

                heldObject = null;
            }
        }
        else
        {
            heldObject.transform.SetParent(null);

            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            heldObject.GetComponent<Collider>().enabled = true;

            heldObject.GetComponent<PlantState>().isBeingHeld = false;

            rb.AddForce(transform.forward * 0.01f, ForceMode.Impulse);
            heldObject.transform.localScale = new Vector3(5, 5, 5);
            heldObject.transform.localRotation = Quaternion.identity;

            Debug.Log("Planta largada fora do compartimento");

            heldObject = null;
        }
        
    }

    public CompartimentoState getCompartimentoState(Transform compartimento)
    {

        CompartimentoState compartimentoStateAux = null;
        if (compartimento.transform.parent.name.StartsWith("Cube"))
        {
            compartimentoStateAux = compartimento.transform.parent.GetComponent<CompartimentoState>();
        }
        else
        {
            Transform parentAux = compartimento.transform.parent;
            Transform childAux = parentAux.transform.GetChild(0);
            compartimentoStateAux = childAux.transform.GetChild(0).GetComponent<CompartimentoState>();
        }

        return compartimentoStateAux;
    }

}
