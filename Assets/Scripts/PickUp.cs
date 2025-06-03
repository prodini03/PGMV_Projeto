using UnityEngine;
using System.Collections.Generic;

public class ArmPickup : MonoBehaviour
{

    [SerializeField] private float detectionRadius = 0.5f;
    [SerializeField] private LayerMask compartimentoLayer;

    private List<GameObject> objectsInReach = new List<GameObject>();
    private GameObject heldObject;
    private GameObject compartimento;
    private CompartimentoState compartimentoState;

    private bool isCompartimento = false;
    private bool playerInZone = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (heldObject == null && objectsInReach.Count > 0 && playerInZone)
            {
                GameObject target = GetFirstValidObject();
                if (target != null)
                {
                    PickUp(target);
                }
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
            PlantState state = other.GetComponent<PlantState>();
            if (state != null && !state.isBeingHeld)
            {
                if (state.isStored)
                {
                    Transform compartimentoTransform = other.transform.parent;
                    CompartimentoState cState = getCompartimentoState(compartimentoTransform);

                    if (cState != null && !cState.isOpen)
                    {
                        return;
                    }
                }

                if (!objectsInReach.Contains(other.gameObject))
                {
                    objectsInReach.Add(other.gameObject);
                }
            }
        }
        else if (other.CompareTag("Compartimento"))
        {
            isCompartimento = true;
            compartimento = other.gameObject;
        }
        Debug.Log("Entraste no trigger do pickup");
        playerInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            objectsInReach.Remove(other.gameObject);
        }
        Debug.Log("Saiste do trigger do pickup");
        playerInZone = false;
    }

    void PickUp(GameObject obj)
    {
        PlantState state = obj.GetComponent<PlantState>();
        if (state != null && state.isStored)
        {
            Transform compartimentoTransform = obj.transform.parent;
            CompartimentoState cState = getCompartimentoState(compartimentoTransform);

            if (cState != null && !cState.isOpen)
            {
                Debug.Log("Tentativa de apanhar planta em compartimento fechado → BLOQUEADO");
                return;
            }
        }

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
            if (compartimentoState != null)
            {
                if (compartimentoState.isOpen)
                {
                    heldObject = obj;
                    heldObject.transform.SetParent(transform, true);
                    heldObject.transform.localScale = new Vector3(0.468f, 0.27f, 0.27f);
                    heldObject.transform.localRotation = Quaternion.AngleAxis(0, new Vector3(1, 1, 1));
                    heldObject.transform.localRotation = Quaternion.AngleAxis(80, new Vector3(1, 0, 0));
                    heldObject.transform.localPosition = new Vector3(0.25f, -0.3f, -0.04f);
                    heldObject.GetComponent<Rigidbody>().isKinematic = true;
                    heldObject.GetComponent<Collider>().enabled = false;
                    heldObject.GetComponent<PlantState>().isBeingHeld = true;
                    heldObject.GetComponent<PlantState>().isStored = false;
                }
            }
            else
            {
                heldObject = obj;
                heldObject.transform.SetParent(transform, true);
                heldObject.transform.localScale = new Vector3(0.468f, 0.27f, 0.27f);
                heldObject.transform.localRotation = Quaternion.AngleAxis(0, new Vector3(1, 1, 1));
                heldObject.transform.localRotation = Quaternion.AngleAxis(80, new Vector3(1, 0, 0));
                heldObject.transform.localPosition = new Vector3(0.25f, -0.3f, -0.04f);
                heldObject.GetComponent<Rigidbody>().isKinematic = true;
                heldObject.GetComponent<Collider>().enabled = false;
                heldObject.GetComponent<PlantState>().isBeingHeld = true;
                heldObject.GetComponent<PlantState>().isStored = false;
            }
        }
        else
        {
            heldObject = obj;
            heldObject.transform.SetParent(transform, true);
            heldObject.transform.localScale = new Vector3(0.468f, 0.27f, 0.27f);
            heldObject.transform.localRotation = Quaternion.AngleAxis(0, new Vector3(1, 1, 1));
            heldObject.transform.localRotation = Quaternion.AngleAxis(80, new Vector3(1, 0, 0));
            heldObject.transform.localPosition = new Vector3(0.25f, -0.3f, -0.04f);
            heldObject.GetComponent<Rigidbody>().isKinematic = true;
            heldObject.GetComponent<Collider>().enabled = false;
            heldObject.GetComponent<PlantState>().isBeingHeld = true;
            heldObject.GetComponent<PlantState>().isStored = false;
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
                        heldObject.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 1));
                        heldObject.transform.localPosition = new Vector3(-0.12f, 0.24f, -2.44f);
                    }
                    else if (compartimento.name.EndsWith("GavetaTop"))
                    {
                        heldObject.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 1));
                        heldObject.transform.localPosition = new Vector3(-0.12f, 0.24f, -0.662f);
                    }

                    else if (compartimento.name.EndsWith("CubiculoTopDir"))
                    {
                        heldObject.transform.localPosition = new Vector3(1f, -0.5f, 0f);
                        heldObject.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
                    }
                    else if (compartimento.name.EndsWith("CubiculoTopEsq"))
                    {
                        heldObject.transform.localPosition = new Vector3(1f, -0.5f, 0f);
                        heldObject.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
                    }
                    else if (compartimento.name.EndsWith("CubiculoBotEsq"))
                    {
                        heldObject.transform.localPosition = new Vector3(1f, -0.5f, 0f);
                        heldObject.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
                    }
                    else if (compartimento.name.EndsWith("CubiculoBotDir"))
                    {
                        heldObject.transform.localPosition = new Vector3(1f, -0.5f, 0f);
                        heldObject.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
                    }

                    Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                    rb.isKinematic = true;

                    Debug.Log("Planta largada dentro de compartimento");

                    heldObject.GetComponent<Collider>().enabled = false;

                    heldObject.GetComponent<PlantState>().isBeingHeld = false;
                    heldObject.GetComponent<PlantState>().isStored = true;
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
                    heldObject.transform.localPosition = new Vector3(-0.35f, -0.265f, 0f);
                    heldObject.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
                }
                if (compartimento.name.EndsWith("PrateleiraTop"))
                {
                    heldObject.transform.localPosition = new Vector3(-0.35f, -0.273f, 0f);
                    heldObject.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
                }
                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                heldObject.GetComponent<Collider>().enabled = false;

                heldObject.GetComponent<PlantState>().isBeingHeld = false;
                heldObject.GetComponent<PlantState>().isStored = true;

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
            heldObject.GetComponent<PlantState>().isStored = false;

            rb.AddForce(transform.forward * 0.01f, ForceMode.Impulse);
            heldObject.transform.localScale = new Vector3(1f, 1f, 1f);
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

    private GameObject GetFirstValidObject()
    {
        foreach (GameObject obj in objectsInReach)
        {
            PlantState state = obj.GetComponent<PlantState>();
            if (state != null && !state.isBeingHeld)
            {
                if (state.isStored)
                {
                    Transform compartimentoTransform = obj.transform.parent;
                    CompartimentoState cState = getCompartimentoState(compartimentoTransform);

                    if (cState != null && !cState.isOpen)
                    {
                        continue;
                    }
                }

                return obj;
            }
        }

        return null;
    }
}
