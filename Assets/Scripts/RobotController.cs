using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RobotController : MonoBehaviour
{
    public float verticalForce = 10f;
    [SerializeField] float turnRate = 1000f;
    [SerializeField] float speed = 1000f;

    [Header("Visual Bobbing")]
    public float hoverAmplitude = 0.2f;       // Height of bob
    public float hoverFrequency = 1.5f;       // Speed of bob

    private Rigidbody rb;
    private Vector3 basePosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        basePosition = transform.position;
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        rb.AddForce(transform.forward * moveInput * speed * Time.deltaTime);

        float turnInput = Input.GetAxis("Horizontal");
        rb.AddTorque(Vector3.up * turnInput * turnRate * Time.deltaTime);

        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * verticalForce);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            rb.AddForce(Vector3.down * verticalForce);
        }
    }

    void Update()
    {
        // Visual-only hover effect (small up/down motion)
        float hoverOffset = Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        transform.position = new Vector3(
            transform.position.x,
            basePosition.y + hoverOffset,
            transform.position.z
        );
    }
}
