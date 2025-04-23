using System;
using System.Collections;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class ArmarioGenerator : MonoBehaviour
{
    public GameObject armarioPrefab;
    public GameObject prateleiraPrefab;
    public GameObject cubiculoPrefab;
    public GameObject gavetaPrefab;

    void Start()
    {
        GerarArmario("Assets/Scripts/Armario.xml");
    }

    void GerarArmario(string path)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);

        XmlNode armarioNode = xmlDoc.SelectSingleNode("/Armario");
        int n = int.Parse(armarioNode.Attributes["n"].Value); 
        int m = int.Parse(armarioNode.Attributes["m"].Value);

        float moduloLargura = 2.0f;  
        float moduloAltura = 2.0f;

        float armarioProfundidade = 2.50f;
        float armarioAltura = (float)m * 2.2f;  
        float armarioLargura = (float)n * 2.18f; 

        GameObject armario = Instantiate(armarioPrefab);
        armario.transform.position = Vector3.zero;
        armario.transform.localScale = new Vector3(armarioProfundidade, armarioAltura, armarioLargura);
        armario.transform.localPosition = Vector3.zero;

        Vector3 armarioPos = armario.transform.position;

        float yOrigin = armarioPos.y;
        float zOrigin = armarioPos.z + (n);

        GameObject modulos = new GameObject("modulos");
        modulos.transform.parent = armario.transform;
        modulos.transform.position = new Vector3(armarioPos.x + 0.49f, yOrigin + 2 + (m/2 * moduloAltura * 2), zOrigin - (n/2 * moduloLargura * 2));


        XmlNodeList linhas = armarioNode.SelectNodes("Linha");
        for (int i = 0; i < n; i++) 
        {
            XmlNode linhaNode = linhas[i];
            for (int j = 0; j < m; j++) 
            {
                XmlNode moduloNode = linhaNode.SelectNodes("Modulo")[j];
                string tipo = moduloNode.Attributes["tipo"].Value;
                GameObject moduloPrefab = tipo switch
                {
                    "P" => prateleiraPrefab,
                    "C" => cubiculoPrefab,
                    "G" => gavetaPrefab,
                    _ => null
                };

                float xPos = armarioPos.x + 0.49f; 
                float yPos = yOrigin + 2 + (j * moduloAltura*2);
                float zPos = zOrigin - (i * moduloLargura*2);

                if (tipo == "G")
                {
                    GameObject modulo = Instantiate(moduloPrefab);
                    modulo.transform.localRotation = Quaternion.AngleAxis(-90, new Vector3(0, 1, 0));
                    modulo.transform.parent = modulos.transform;
                    modulo.name = "Modulo_" + i + "_" + j;
                    modulo.transform.position = new Vector3(xPos, yPos, zPos);
                }
                else
                {
                    GameObject modulo = Instantiate(moduloPrefab);
                    modulo.transform.parent = modulos.transform;
                    modulo.name = "Modulo_" + i + "_" + j;
                    modulo.transform.position = new Vector3(xPos, yPos, zPos);
                }

                
                
            }
        }
        armario.transform.position = Vector3.zero;
        modulos.transform.position = new Vector3(armarioPos.x + 0.49f, 0, 0);
    }
}
