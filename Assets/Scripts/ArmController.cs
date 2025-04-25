using UnityEngine;

public class ArmController : MonoBehaviour
{
    public float shoulderRotationSpeed = 40f;
    public float shoulderMaxAngle = 0f;
    public float shoulderMinAngle = -70f;

    public float elbowRotationSpeed = 40f;
    public float elbowMaxAngle = 90f;
    public float elbowMinAngle = 0f;

    private Transform shoulder;
    private Transform elbow;

    private float shoulderCurrentAngle = 0f;
    private float elbowCurrentAngle = 0f;

    void Start()
    {
        RobotGen robot = GetComponent<RobotGen>();
        if (robot != null)
        {

            shoulder = robot.leftElbow.parent; 
            elbow = robot.leftElbow;
        }

        if (shoulder == null || elbow == null)
        {
            Debug.LogError("Shoulder or Elbow reference is null. Check RobotGen setup.");
        }
    }

    void Update()
    {
        if (shoulder == null || elbow == null) return;

        float shoulderDelta = 0f;

        if (Input.GetKey(KeyCode.E))
        {
            shoulderDelta = -shoulderRotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            shoulderDelta = shoulderRotationSpeed * Time.deltaTime;
        }

        shoulderCurrentAngle = Mathf.Clamp(shoulderCurrentAngle + shoulderDelta, shoulderMinAngle, shoulderMaxAngle);
        transform.GetChild(2).GetChild(0).transform.localRotation = Quaternion.AngleAxis(shoulderCurrentAngle, new Vector3(1,0,0));

        elbowCurrentAngle = Mathf.Clamp(-shoulderCurrentAngle * 0.5f, elbowMinAngle, elbowMaxAngle);
        transform.GetChild(2).GetChild(0).GetChild(1).transform.localRotation = Quaternion.AngleAxis(-elbowCurrentAngle, new Vector3(1, 0, 0));
    }
}
