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

    public void Interpret(string sequence)
    {
        transformStack = new Stack<TransformInfo>();
        Transform turtle = new GameObject("Turtle").transform;
        turtle.position = transform.position;
        turtle.rotation = transform.rotation;

        foreach (char c in sequence)
        {
            switch (c)
            {
                case 'F':
                    GameObject branch = Instantiate(branchPrefab, turtle.position, turtle.rotation, this.transform);
                    branch.transform.localScale = new Vector3(branchThickness, length / 2f, branchThickness);
                    branch.transform.Translate(Vector3.up * length / 2f);
                    turtle.Translate(Vector3.up * length);
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
                    GameObject flower = Instantiate(flowerPrefab, turtle.position, turtle.rotation, this.transform);
                    flower.transform.localScale = new Vector3(length * 250f, length * 250f, length * 250f);
                    flower.transform.Rotate(Vector3.right, -90f, Space.Self);
                    flower.transform.Translate(Vector3.forward * -0.03f, Space.Self); // Small fixed Z translation
                    flower.transform.Translate(Vector3.forward * length / 2f, Space.Self);
                    break;
                case '/':
                    turtle.Rotate(Vector3.up * angle);
                    break;
                case '\\':
                    turtle.Rotate(Vector3.up * -angle); 
                    break;
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
