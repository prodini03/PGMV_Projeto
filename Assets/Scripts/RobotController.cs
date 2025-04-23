using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RobotController : MonoBehaviour
{

    public float verticalForce = 10f;
    [SerializeField] float turnRate = 1000f;
    [SerializeField] float speed = 1000f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
}