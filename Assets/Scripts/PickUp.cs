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
    private int refreshCounter = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (heldObject == null && objectsInReach.Count > 0 && playerInZone)
            {
                GameObject target = GetClosestValidObject();
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
        playerInZone = true;

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            //objectsInReach.Remove(other.gameObject);
        }
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
                GameObject modulo = getModuloFromCompartimento(compartimento);
                DoorsState doorsState = modulo.transform.parent.gameObject.transform.parent.GetComponent<DoorsState>();
                if (getModuloFromCompartimento(compartimento).name.StartsWith("Modulo_R"))
                {
                    if (doorsState.rightDoorOpen == true && doorsState.leftDoorOpen == false)
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
                else if (getModuloFromCompartimento(compartimento).name.StartsWith("Modulo_L"))
                {
                    if (doorsState.rightDoorOpen == false && doorsState.leftDoorOpen == true)
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
                if (colliders[i].name.StartsWith("DoorPlantCollider"))
                    compartimento = colliders[i].transform;
            }
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

                    heldObject.GetComponent<Collider>().enabled = false;

                    heldObject.GetComponent<PlantState>().isBeingHeld = false;
                    heldObject.GetComponent<PlantState>().isStored = true;
                    heldObject = null;
                }
            }
            else
            {
                GameObject modulo = getModuloFromCompartimento(compartimento);
                DoorsState doorsState = modulo.transform.parent.gameObject.transform.parent.GetComponent<DoorsState>();
                if (getModuloFromCompartimento(compartimento).name.StartsWith("Modulo_R"))
                {
                    if (doorsState.rightDoorOpen == true && doorsState.leftDoorOpen == false)
                    {
                        
                        if (compartimento.name.EndsWith("PrateleiraBot"))
                        {
                            heldObject.transform.SetParent(compartimento);
                            heldObject.transform.localPosition = Vector3.zero;
                            heldObject.transform.localRotation = Quaternion.identity;

                            heldObject.transform.localPosition = new Vector3(-0.35f, -0.265f, 0f);
                            heldObject.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));

                            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                            rb.isKinematic = true;
                            heldObject.GetComponent<Collider>().enabled = false;

                            heldObject.GetComponent<PlantState>().isBeingHeld = false;
                            heldObject.GetComponent<PlantState>().isStored = true;

                            heldObject = null;
                        }
                    }
                }
                else if (getModuloFromCompartimento(compartimento).name.StartsWith("Modulo_L"))
                {
                    if (doorsState.rightDoorOpen == false && doorsState.leftDoorOpen == true)
                    {
                       
                        if (compartimento.name.EndsWith("PrateleiraTop"))
                        {
                            heldObject.transform.SetParent(compartimento);
                            heldObject.transform.localPosition = Vector3.zero;
                            heldObject.transform.localRotation = Quaternion.identity;

                            heldObject.transform.localPosition = new Vector3(-0.35f, -0.273f, 0f);
                            heldObject.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));

                            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                            rb.isKinematic = true;
                            heldObject.GetComponent<Collider>().enabled = false;

                            heldObject.GetComponent<PlantState>().isBeingHeld = false;
                            heldObject.GetComponent<PlantState>().isStored = true;

                            heldObject = null;
                        }
                    }
                }
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
            heldObject.transform.localRotation = Quaternion.identity;

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

    private GameObject GetClosestValidObject()
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject obj in objectsInReach)
        {
            if (obj == null) continue;

            PlantState state = obj.GetComponent<PlantState>();
            if (state != null && !state.isBeingHeld)
            {
                if (state.isStored)
                {
                    Transform compartimentoTransform = obj.transform.parent;
                    CompartimentoState cState = getCompartimentoState(compartimentoTransform);
                    if (cState != null && !cState.isOpen)
                        continue;
                }

                float dist = Vector3.Distance(transform.position, obj.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = obj;
                }
            }
        }

        return closest;
    }

    public void ForceRegister(GameObject obj)
    {
        if (!objectsInReach.Contains(obj) && obj.CompareTag("Pickup"))
        {
            PlantState state = obj.GetComponent<PlantState>();
            if (state != null && !state.isBeingHeld)
            {
                objectsInReach.Add(obj);
            }
        }
    }

    public GameObject getModuloFromCompartimento(Transform compartimento)
    {
        GameObject modulo = compartimento.gameObject;


        while (!modulo.name.StartsWith("Modulo"))
        {
            modulo = modulo.transform.parent?.gameObject;
        }

        return modulo;
    }

}
