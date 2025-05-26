using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
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
                }
                else
                {
                    GameObject modulo = Instantiate(moduloPrefab);
                    if (i == 0 || i < n / 2) modulo.name = "Modulo_L_" + j + "_" + i;
                    else modulo.name = "Modulo_R_" + j + "_" + i;
                    modulo.transform.parent = armario.transform;
                    modulo.transform.position = new Vector3(xPos, yPos, zPos);

                }
                

            }
        }
        armario.transform.localRotation = Quaternion.AngleAxis(r, new Vector3(0, 1, 0));
        armario.transform.position = new Vector3(x,y,z);
    }
}
