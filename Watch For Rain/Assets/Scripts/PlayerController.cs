using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    // Movement variables
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float maxJumpHeight = 15f;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isJumping = false;
    private float currentJumpForce;

    // Mobile controls
    private bool moveLeft = false;
    private bool moveRight = false;

    // Ground detection
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        float horizontalInput = 0;

        // PC Controls
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.isEditor)
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }
        // Mobile Controls
        else
        {
            if (moveLeft) horizontalInput = -1;
            if (moveRight) horizontalInput = 1;
        }

        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Flip the character sprite
        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), transform.localScale.y, transform.localScale.z);
        }
    }

    private void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && !isJumping && (Input.GetButtonDown("Jump") || moveLeft || moveRight))
        {
            isJumping = true;
            currentJumpForce = 0; // Reset jump force
        }

        if (isJumping)
        {
            if (Input.GetButton("Jump") || moveLeft || moveRight)
            {
                currentJumpForce += Time.deltaTime * jumpForce;
                currentJumpForce = Mathf.Clamp(currentJumpForce, 0, maxJumpHeight);

                rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
            }

            if (Input.GetButtonUp("Jump") || currentJumpForce >= maxJumpHeight)
            {
                isJumping = false;
            }
        }
    }

    // Mobile Button Functions
    public void OnMoveLeft(bool isPressed)
    {
        moveLeft = isPressed;
    }

    public void OnMoveRight(bool isPressed)
    {
        moveRight = isPressed;
    }

    public void OnJump(bool isPressed)
    {
        if (isPressed && isGrounded)
        {
            isJumping = true;
            currentJumpForce = 0;
        }
    }

    void OnDrawGizmos()
    {
        // Visualize ground check radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
