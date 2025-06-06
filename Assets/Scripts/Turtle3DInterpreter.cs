using System.Collections.Generic;
using UnityEngine;

public class Turtle3DInterpreter : MonoBehaviour
{
    public GameObject branchPrefab;
    public GameObject leafPrefab;
    public GameObject flowerPrefab;

    public float length = 1f;
    public float angle = 25f;
    public float branchThickness = 0.1f;
    public float alturaMax = 1.50f;

    public Vector3 worldCenter = Vector3.one;
    public Vector3 worldSize = Vector3.one;

    private class SymbolState
    {
        public Vector3 position;
        public Quaternion rotation;
        public char symbol;
    }

    public void Interpret(string sequence, bool includeFlowers)
    {
        Stack<TransformInfo> transformStack = new();
        Transform turtle = new GameObject("Turtle").transform;
        turtle.position = transform.position;
        turtle.rotation = transform.rotation;

        List<SymbolState> flowerMarkers = new();
        List<Vector3> branchPositions = new();

        for (int i = 0; i < sequence.Length; i++)
        {
            char c = sequence[i];

            switch (c)
            {
                case 'F':
                    GameObject branch = Instantiate(branchPrefab, turtle.position, turtle.rotation, transform);
                    branch.transform.localScale = new Vector3(branchThickness, length / 2f, branchThickness);
                    branch.transform.Translate(Vector3.up * length / 2f);
                    turtle.Translate(Vector3.up * length);
                    branchPositions.Add(branch.transform.position);
                    break;

                case '+': turtle.Rotate(Vector3.forward * angle); break;
                case '-': turtle.Rotate(Vector3.forward * -angle); break;
                case '&': turtle.Rotate(Vector3.right * angle); break;
                case '^': turtle.Rotate(Vector3.right * -angle); break;
                case '/': turtle.Rotate(Vector3.up * angle); break;
                case '\\': turtle.Rotate(Vector3.up * -angle); break;

                case '[': transformStack.Push(new TransformInfo(turtle.position, turtle.rotation)); break;
                case ']':
                    var ti = transformStack.Pop();
                    turtle.position = ti.position;
                    turtle.rotation = ti.rotation;
                    break;

                case 'L':
                    GameObject leaf = Instantiate(leafPrefab, turtle.position, turtle.rotation, transform);
                    leaf.transform.localScale = new Vector3(length * 125f, length * 255f, length * 125f);
                    leaf.transform.Translate(Vector3.forward * length / 2.5f, Space.Self);
                    break;

                case 'X':
                    if (includeFlowers)
                    {
                        flowerMarkers.Add(new SymbolState { position = turtle.position, rotation = turtle.rotation, symbol = 'X' });
                    }
                    break;
            }
        }

        if (includeFlowers)
        {
            foreach (var marker in flowerMarkers)
            {
                bool blocked = false;
                foreach (var pos in branchPositions)
                {
                    float hDist = Vector2.Distance(new Vector2(pos.x, pos.z), new Vector2(marker.position.x, marker.position.z));
                    float vOffset = pos.y - marker.position.y;
                    if (hDist < 0.1f && vOffset > 0f && vOffset < length * 3f)
                    {
                        blocked = true;
                        break;
                    }
                }

                if (!blocked)
                {
                    GameObject flower = Instantiate(flowerPrefab, marker.position, marker.rotation, transform);
                    flower.transform.localScale = Vector3.one * length * 160f;
                    flower.transform.Rotate(Vector3.right, -90f, Space.Self);
                    flower.transform.Translate(Vector3.forward * -0.12f + Vector3.up * 0.01f + Vector3.forward * length / 2f, Space.Self);
                }
            }

            float minY = float.MaxValue, maxY = float.MinValue;
            float minX = float.MaxValue, maxX = float.MinValue;
            float minZ = float.MaxValue, maxZ = float.MinValue;
            foreach (Transform child in transform)
            {
                Vector3 pos = child.position;

                if (pos.y < minY) minY = pos.y;
                if (pos.y > maxY) maxY = pos.y;

                if (pos.x < minX) minX = pos.x;
                if (pos.x > maxX) maxX = pos.x;

                if (pos.z < minZ) minZ = pos.z;
                if (pos.z > maxZ) maxZ = pos.z;
            }

        float altura = maxY - minY;
        float largura = maxX - minX;
        float profundidade = maxZ - minZ;


        float maxAltura = 1.5f;
        float maxLargura = 1.5f;
        float maxProfundidade = 1.5f;


        float scaleY = altura > maxAltura ? maxAltura / altura : 1f;
        float scaleX = largura > maxLargura ? maxLargura / largura : 1f;
        float scaleZ = profundidade > maxProfundidade ? maxProfundidade / profundidade : 1f;


        float scale = Mathf.Min(scaleX, scaleY, scaleZ);

        if (scale < 1f)
        {
            transform.localScale *= scale;
            Debug.Log("Planta foi escalada por: " + scale);


            worldCenter = new Vector3((minX + maxX) / 2f * scale, (minY + maxY) / 2f * scale, (minZ + maxZ) / 2f * scale);
            worldSize = new Vector3((maxX - minX) * scale, (maxY - minY) * scale, (maxZ - minZ) * scale);
        }


        worldCenter = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, (minZ + maxZ) / 2f);
        worldSize = new Vector3(maxX - minX, maxY - minY, maxZ - minZ);
        }

        Destroy(turtle.gameObject);
    }

    private struct TransformInfo
    {
        public Vector3 position;
        public Quaternion rotation;
        public TransformInfo(Vector3 pos, Quaternion rot) { position = pos; rotation = rot; }
    }
}
