using UnityEngine;

public class RobotController : MonoBehaviour
{
    public float speed = 2500f;
    public float verticalForce = 10f;

    public float hoverForce = 30f;
    public float hoverFrequency = 2f;

    public float mouseSensitivity = 2f;

    private Rigidbody rb;

    private float horizontalCameraMove;
    private float verticalCameraMove;

    private Transform headTransform;
    private Quaternion initialHeadLocalRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        headTransform = transform.Find("Head");

        if (headTransform != null)
        {
            initialHeadLocalRotation = headTransform.localRotation;
        }
        else
        {
            Debug.LogWarning("Head not found! Make sure it’s named 'Head' or tagged as 'Head'.");
        }
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
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        horizontalCameraMove += mouseX;
        verticalCameraMove -= mouseY;
        verticalCameraMove = Mathf.Clamp(verticalCameraMove, -30f, 30f);

        Quaternion targetRotation = Quaternion.Euler(0f, horizontalCameraMove, 0f);
        rb.MoveRotation(targetRotation);

        headTransform.localRotation = initialHeadLocalRotation * Quaternion.Euler(0f, verticalCameraMove, 0f);
        
    }
}
