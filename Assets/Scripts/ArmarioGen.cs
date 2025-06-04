using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ArmarioGen : MonoBehaviour
{
    [SerializeField] GameObject armarioPrefab;
    [SerializeField] GameObject gavetaPrefab;
    [SerializeField] GameObject prateleiraPrefab;
    [SerializeField] GameObject cubiculoPrefab;
    [SerializeField] string fileName;
    
    void Start()
    {
        GerarArmario("Assets/Scripts/" + fileName);
    }

    void GerarArmario(string path)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);

        XmlNode armarioNode = xmlDoc.SelectSingleNode("/Armario");
        int n = int.Parse(armarioNode.Attributes["n"].Value);
        int m = int.Parse(armarioNode.Attributes["m"].Value);
        float x = float.Parse(armarioNode.Attributes["x"].Value);
        float y = float.Parse(armarioNode.Attributes["y"].Value);
        float z = float.Parse(armarioNode.Attributes["z"].Value);
        float r = float.Parse(armarioNode.Attributes["r"].Value);

        if (n % 2 != 0) 
        { 
            throw new System.Exception("O armário não pode ter um número ímpar em n");
        }

        float moduloLargura = 2.0f;  
        float moduloAltura = 2.0f;
        float armarioProfundidade = 2.50f;
        float armarioAltura = (float)m * 2.2f;
        float armarioLargura = (float)n * 2.18f;

        GameObject armario = Instantiate(armarioPrefab);
        armario.transform.parent = transform;
        armario.transform.position = Vector3.zero;
        armario.transform.localScale = new Vector3(armarioProfundidade, armarioAltura, armarioLargura);
        armario.transform.localPosition = Vector3.zero;

        Vector3 armarioPos = armario.transform.position;

        float startPointModuleX = armarioPos.x + 0.48f;
        float startPointModuleY = armarioPos.y - armarioAltura + (float)m*0.20f;
        float startPointModuleZ = armarioPos.z + armarioLargura  - (float)n * 0.18f;

        XmlNodeList linhas = armarioNode.SelectNodes("Coluna");

        if(linhas.Count != n) throw new System.Exception("Erro ao criar ficheiro xml, número de colunas diferente de n");

        for (int i = 0; i < n; i++)
        {
            XmlNode linhaNode = linhas[i];
            for (int j = 0; j < m; j++)
            {
                XmlNode moduloNode = linhaNode.SelectNodes("Modulo")[j];
                int moduloCount = linhaNode.SelectNodes("Modulo").Count;

                if (moduloCount != m) throw new System.Exception("Erro ao criar ficheiro xml, número de linhas diferente de m");

                string tipo = moduloNode.Attributes["tipo"].Value;
                string planta = moduloNode.Attributes["planta"]?.Value;
               

                GameObject moduloPrefab = tipo switch
                {
                    "P" => prateleiraPrefab,
                    "C" => cubiculoPrefab,
                    "G" => gavetaPrefab,
                    _ => null
                };

                float xPos = startPointModuleX; 
                float yPos = startPointModuleY + 2 + (j * moduloAltura*2);
                float zPos = startPointModuleZ - 2 - (i * moduloLargura*2);

                if (tipo == "G")
                {
                    GameObject modulo = Instantiate(moduloPrefab);
                    if(i == 0 || i < n/2) modulo.name = "Modulo_L_" + j + "_" + i;
                    else modulo.name = "Modulo_R_" + j + "_" + i;
                    modulo.transform.parent = armario.transform;
                    modulo.transform.position = new Vector3(xPos, yPos, zPos);

                    if (planta == "s")
                    {
                        string tipoPlanta = moduloNode.Attributes["tipoPlanta"].Value;
                        string moduloPos = moduloNode.Attributes["pos"].Value;
                        CriarPlantaNoModulo(modulo, tipoPlanta, moduloPos, tipo);
                    }
                }
                else
                {
                    GameObject modulo = Instantiate(moduloPrefab);
                    if (i == 0 || i < n / 2) modulo.name = "Modulo_L_" + j + "_" + i;
                    else modulo.name = "Modulo_R_" + j + "_" + i;
                    modulo.transform.parent = armario.transform;
                    modulo.transform.position = new Vector3(xPos, yPos, zPos);

                    if (planta == "s")
                    {
                        string tipoPlanta = moduloNode.Attributes["tipoPlanta"].Value;
                        string moduloPos = moduloNode.Attributes["pos"].Value;
                        CriarPlantaNoModulo(modulo, tipoPlanta, moduloPos, tipo);
                    }

                }
                
            }
        }
        armario.transform.localRotation = Quaternion.AngleAxis(r, new Vector3(0, 1, 0));
        armario.transform.position = new Vector3(x,y,z);
    }

    void CriarPlantaNoModulo(GameObject moduloGO, string tipoPlanta, string ModuloPos, string ModuloName)
    {
        

        GameObject plantaGO = new GameObject("Planta_" + tipoPlanta);

        Rigidbody rb = plantaGO.AddComponent<Rigidbody>();
        rb.mass = 0.07f;
        rb.drag = 0;
        rb.angularDrag = 0.05f;
        rb.useGravity = true; 
        rb.isKinematic = true;

        plantaGO.AddComponent<BoxCollider>();
        FixBoxColliderToPlantSize(plantaGO);
        plantaGO.GetComponent<Collider>().enabled = false;

        plantaGO.AddComponent<PlantState>();
        plantaGO.GetComponent<PlantState>().isStored = true;

        plantaGO.tag = "Pickup";

        plantaGO.transform.parent = getTransformPos(ModuloName, ModuloPos, moduloGO);
        plantaGO.transform.localPosition = getVectorForPos(ModuloName, ModuloPos);

        if(ModuloName == "G")
        {
            plantaGO.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));
        }
        else
        {
            plantaGO.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
        }

        var lsys = plantaGO.AddComponent<LSystemController>();

        switch (tipoPlanta)
        {
            case "bamboo":
                lsys.selectedPlant = LSystemController.PlantType.Bamboo;
                break;
            case "flor":
                lsys.selectedPlant = LSystemController.PlantType.FloweringBush;
                break;
            default:
                Debug.LogWarning("Tipo de planta desconhecido: " + tipoPlanta);
                Destroy(plantaGO);
                return;
        }

        GameObject robot = GameObject.FindGameObjectWithTag("Player");
        Transform leftElbow = robot.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetChild(0);
        ArmPickup pickup = leftElbow.GetComponent<ArmPickup>();
        pickup?.ForceRegister(plantaGO);

    }
    public void FixBoxColliderToPlantSize(GameObject plantaGO)
    {
        StartCoroutine(FixColliderNextFrame(plantaGO));
    }

    private IEnumerator FixColliderNextFrame(GameObject plantaGO)
    {

        yield return null;

        Vector3 worldCenter = plantaGO.GetComponent<Turtle3DInterpreter>().worldCenter;
        Vector3 worldSize = plantaGO.GetComponent<Turtle3DInterpreter>().worldSize;

        Vector3 localCenter = plantaGO.transform.InverseTransformPoint(worldCenter);
        Vector3 localSize = plantaGO.transform.InverseTransformVector(worldSize);

        BoxCollider box = plantaGO.GetComponent<BoxCollider>();
        
        box.center = new Vector3(localCenter.x, localCenter.y, localCenter.z);
        box.size = new Vector3(worldSize.x, worldSize.y, worldSize.z);
        print(new Vector3(worldSize.x, worldSize.y, worldSize.z));

        Debug.Log("BoxCollider ajustado com sucesso.");
    }






    public Transform getTransformPos(string ModuloName, string ModuloPos, GameObject modulo)
    {
        Transform pos = null;

        switch (ModuloName)
        {
            case "C":
                switch (ModuloPos)
                {
                    case "TopEsq":
                         pos = modulo.transform.GetChild(0).GetChild(1);
                        break;

                    case "TopDir":
                        pos = modulo.transform.GetChild(1).GetChild(1);
                        break;

                    case "BotEsq":
                        pos = modulo.transform.GetChild(2).GetChild(1);
                        break;

                    case "BotDir":
                        pos = modulo.transform.GetChild(3).GetChild(1);
                        break;
                }
                break;
            case "G":
                switch (ModuloPos)
                {
                    case "Top":
                        pos = modulo.transform.GetChild(0).GetChild(1);
                        break;

                    case "Bot":
                        pos = modulo.transform.GetChild(1).GetChild(1);
                        break;
                }
                break;

            case "P":
                switch(ModuloPos)
                {
                    case "Top":
                        pos = modulo.transform.GetChild(1).GetChild(0);
                        break;

                    case "Bot":
                        pos = modulo.transform.GetChild(0).GetChild(0);
                        break;
                    }
                    break;
        }

        return pos;
    }

    public Vector3 getVectorForPos(string ModuloName, string ModuloPos)
    {
        Vector3 pos = Vector3.zero;

        switch (ModuloName)
        {
            case "C":
                switch (ModuloPos)
                {
                    case "TopEsq":
                        pos = new Vector3(1.1f, -0.46f, 0f);
                        break;

                    case "TopDir":
                        pos = new Vector3(1.1f, -0.46f, 0f);
                        break;

                    case "BotEsq":
                        pos = new Vector3(1.1f, -0.46f, 0f);
                        break;

                    case "BotDir":
                        pos = new Vector3(1.1f, -0.46f, 0f);
                        break;
                }
                break;
            case "G":
                switch (ModuloPos)
                {
                    case "Top":
                        pos = new Vector3(-0.11f, 0.15f, -1.06f);
                        break;

                    case "Bot":
                        pos = new Vector3(-0.11f, 0.15f, -2.455f);
                        break;
                }
                break;

            case "P":
                switch (ModuloPos)
                {
                    case "Top":
                        pos = new Vector3(-0.2f, -0.265f, 0f);
                        break;

                    case "Bot":
                        pos = new Vector3(-0.2f, -0.265f, 0f);
                        break;
                }
                break;
        }

        return pos;
    }
}
