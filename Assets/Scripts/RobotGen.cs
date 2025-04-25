using UnityEngine;

public class RobotGen : MonoBehaviour
{
    public GameObject headPrefab;
    public GameObject bodyPrefab;
    public GameObject jointPrefab;

    [HideInInspector] public Transform leftShoulder;
    [HideInInspector] public Transform leftElbow;

    void Awake()
    {
        // Head
        GameObject head = Instantiate(headPrefab, transform);
        head.name = "Head";
        head.transform.localPosition = new Vector3(0, 1.7f, 0);

        // Body
        GameObject body = Instantiate(bodyPrefab, transform);
        body.name = "Body";
        body.transform.localPosition = new Vector3(0, 0.5f, 0);

        // Left Arm
        GameObject leftArm = new GameObject("LeftArm");
        GameObject leftShoulderInstance = Instantiate(jointPrefab);
        GameObject leftElbowInstance = Instantiate(jointPrefab);

        leftElbowInstance.transform.parent = leftShoulderInstance.transform;
        leftShoulderInstance.transform.parent = leftArm.transform;
        leftArm.transform.parent = transform;

        leftElbowInstance.name = "LeftElbow";
        leftElbowInstance.transform.localPosition = new Vector3(0f, -0.44f, 0);

        leftElbow = leftElbowInstance.transform;

        leftShoulderInstance.name = "LeftShoulder";
        leftShoulderInstance.transform.localPosition = new Vector3(-0.62f, 1f, 0);

        leftShoulder = leftShoulderInstance.transform;



        // Right Arm
        GameObject rightArm = new GameObject("RightArm");
        rightArm.transform.parent = transform;

        GameObject rightShoulder = Instantiate(jointPrefab, rightArm.transform);
        rightShoulder.name = "RightShoulder";
        rightShoulder.transform.localPosition = new Vector3(0.62f, 1f, 0);

        GameObject rightElbow = Instantiate(jointPrefab, rightArm.transform);
        rightElbow.name = "RightElbow";
        rightElbow.transform.localPosition = new Vector3(0.62f, 0.56f, 0);
    }
}
