using UnityEngine;

public class RobotController : MonoBehaviour
{

    public float speed = 1000f;
    public float verticalForce = 10f;

    public float hoverForce = 5f;
    public float hoverFrequency = 2f;

    public float mouseSensitivity = 2f;

    private Rigidbody rb;

    private float horizontalCameraMove;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {

        float moveForward = Input.GetAxis("Vertical");
        float moveRight = Input.GetAxis("Horizontal");
        Vector3 moveDirection = (transform.forward * moveForward + transform.right * moveRight).normalized;
        rb.AddForce(moveDirection * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * verticalForce);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            rb.AddForce(Vector3.down * verticalForce);
        }

        float hoverOffset = Mathf.Sin(Time.time * hoverFrequency) * hoverForce;
        rb.AddForce(Vector3.up * hoverOffset * Time.deltaTime, ForceMode.Force);
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;

        horizontalCameraMove += mouseX;

        transform.rotation = Quaternion.Euler(0f, horizontalCameraMove, 0f);

    }
}
