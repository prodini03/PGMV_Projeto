using System.Collections.Generic;
using UnityEngine;

public class Turtle3DInterpreter : MonoBehaviour
{
    public GameObject branchPrefab;
    public GameObject leafPrefab;
    public GameObject flowerPrefab;

    public float length = 1f;
    public float angle = 25f;
    public float branchThickness = 0.1f; // Valor padrão

    private Stack<TransformInfo> transformStack;

    private class SymbolState
    {
        public Vector3 position;
        public Quaternion rotation;
        public char symbol;
    }

    public void Interpret(string sequence)
    {
        transformStack = new Stack<TransformInfo>();
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
                    GameObject branch = Instantiate(branchPrefab, turtle.position, turtle.rotation, this.transform);
                    branch.transform.localScale = new Vector3(branchThickness, length / 2f, branchThickness);
                    branch.transform.Translate(Vector3.up * length / 2f);
                    turtle.Translate(Vector3.up * length);
                    branchPositions.Add(branch.transform.position);
                    break;
                case '+':
                    turtle.Rotate(Vector3.forward * angle);
                    break;
                case '-':
                    turtle.Rotate(Vector3.forward * -angle);
                    break;
                case '&':
                    turtle.Rotate(Vector3.right * angle);
                    break;
                case '^':
                    turtle.Rotate(Vector3.right * -angle);
                    break;
                case '[':
                    transformStack.Push(new TransformInfo(turtle.position, turtle.rotation));
                    break;
                case ']':
                    TransformInfo ti = transformStack.Pop();
                    turtle.position = ti.position;
                    turtle.rotation = ti.rotation;
                    break;
                case 'L':
                    GameObject leaf = Instantiate(leafPrefab, turtle.position, turtle.rotation, this.transform);
                    leaf.transform.localScale = new Vector3(length * 125f, length * 255f, length * 125f);
                    leaf.transform.Translate(Vector3.forward * length / 2.5f, Space.Self);
                    break;
                case 'X':
                    // Guarda posição e rotação do X para avaliação posterior
                    flowerMarkers.Add(new SymbolState
                    {
                        position = turtle.position,
                        rotation = turtle.rotation,
                        symbol = 'X'
                    });
                    break;
                case '/':
                    turtle.Rotate(Vector3.up * angle);
                    break;
                case '\\':
                    turtle.Rotate(Vector3.up * -angle);
                    break;
            }
        }

        // Verifica quais X têm espaço livre acima (sem ramos por cima)
        foreach (var marker in flowerMarkers)
        {
            bool blockedAbove = false;
            foreach (var pos in branchPositions)
            {
                if (Mathf.Abs(pos.x - marker.position.x) < 0.05f &&
                    Mathf.Abs(pos.z - marker.position.z) < 0.05f &&
                    pos.y > marker.position.y &&
                    Mathf.Abs(pos.y - marker.position.y) < length * 1.2f)
                {
                    blockedAbove = true;
                    break;
                }
            }

            if (!blockedAbove)
            {
                GameObject flower = Instantiate(flowerPrefab, marker.position, marker.rotation, this.transform);
                flower.transform.localScale = new Vector3(length * 160f, length * 160f, length * 160f);
                flower.transform.Rotate(Vector3.right, -90f, Space.Self);
                flower.transform.Translate(Vector3.forward * -0.12f, Space.Self);
                flower.transform.Translate(Vector3.up * 0.01f, Space.Self); 
                flower.transform.Translate(Vector3.forward * length / 2f, Space.Self);
            }
        }

        Destroy(turtle.gameObject);
    }

    struct TransformInfo
    {
        public Vector3 position;
        public Quaternion rotation;
        public TransformInfo(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }
}
