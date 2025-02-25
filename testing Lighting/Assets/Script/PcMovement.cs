using UnityEngine;
using UnityEngine.EventSystems; // Required for mobile button detection

public class PcMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private float horizontalInput = 0f;
    private bool isUsingMobileControls = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Only use keyboard movement if mobile controls are not in use
        if (!isUsingMobileControls)
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }

        // Apply movement
        Vector3 movement = new Vector3(horizontalInput * moveSpeed, rb.velocity.y, 0);
        rb.velocity = movement;

        // Check if player is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);

        // Keyboard jump for PC
        if (!isUsingMobileControls && isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    // Mobile Button Press (Move while button is held)
    public void MoveLeft_Press()
    {
        horizontalInput = -1f;
        isUsingMobileControls = true;
    }

    public void MoveRight_Press()
    {
        horizontalInput = 1f;
        isUsingMobileControls = true;
    }

    public void StopMoving_Release()
    {
        horizontalInput = 0f;
    }

    // Jump for mobile & PC
    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}