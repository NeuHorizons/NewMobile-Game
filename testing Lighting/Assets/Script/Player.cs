using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private Vector2 movementInput;
    private PlayerInput playerInput; // Reference to Unity’s Input System

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>(); // Ensure PlayerInput component is attached
    }

    void Update()
    {
        // Get movement input from the joystick
        movementInput = playerInput.actions["Move"].ReadValue<Vector2>();

        // Move left/right based on joystick X-axis
        Vector3 movement = new Vector3(movementInput.x * moveSpeed, rb.velocity.y, 0);
        rb.velocity = movement;

        // Check if player is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);

        // Jump when joystick is pushed up
        if (isGrounded && movementInput.y > 0.5f) 
        {
            Jump();
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}