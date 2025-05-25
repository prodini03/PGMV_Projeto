using System.Collections.Generic;
using UnityEngine;

public class Turtle3DInterpreter : MonoBehaviour
{
    public GameObject branchPrefab;
    public GameObject leafPrefab;
    public GameObject flowerPrefab;

    public float length = 1f;
    public float angle = 25f;

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
                    branch.transform.localScale = new Vector3(0.1f, length / 2f, 0.1f);
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
                    Instantiate(leafPrefab, turtle.position, turtle.rotation, this.transform);
                    break;
                case 'X':
                    Instantiate(flowerPrefab, turtle.position, turtle.rotation, this.transform);
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
