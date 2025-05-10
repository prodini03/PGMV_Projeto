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
        head.transform.localPosition = new Vector3(0, 4.725f, 0);

        // Body
        GameObject body = Instantiate(bodyPrefab, transform);
        body.name = "Body";
        body.transform.localPosition = new Vector3(0, 0.375f, 0);

        // Left Arm
        GameObject leftArm = new GameObject("LeftArm");
        GameObject leftShoulderInstance = Instantiate(jointPrefab);
        GameObject leftElbowInstance = Instantiate(jointPrefab);

        leftElbowInstance.transform.parent = leftShoulderInstance.transform;
        leftShoulderInstance.transform.parent = leftArm.transform;
        leftArm.transform.parent = transform;

        leftElbowInstance.name = "LeftElbow";
        leftElbowInstance.transform.localPosition = new Vector3(0f, -0.33f, 0);

        leftElbow = leftElbowInstance.transform;

        leftShoulderInstance.name = "LeftShoulder";
        leftShoulderInstance.transform.localPosition = new Vector3(-0.33f, 0.75f, 0);

        leftShoulder = leftShoulderInstance.transform;

        leftArm.transform.localPosition = new Vector3(-1.875f, 2.1375f, 0);



        // Right Arm
        GameObject rightArm = new GameObject("RightArm");
        GameObject rightShoulder = Instantiate(jointPrefab, rightArm.transform);
        GameObject rightElbow = Instantiate(jointPrefab, rightArm.transform);

        rightElbow.transform.parent = rightShoulder.transform;
        rightShoulder.transform.parent = rightElbow.transform;
        rightArm.transform.parent = transform;

        rightElbow.name = "RightElbow";
        rightElbow.transform.localPosition = new Vector3(0f, -0.33f, 0);

        rightShoulder.name = "RightShoulder";
        rightShoulder.transform.localPosition = new Vector3(0.33f, 0.75f, 0);

        rightArm.transform.localPosition = new Vector3(1.875f, 2.1375f, 0);

        
    }
}
