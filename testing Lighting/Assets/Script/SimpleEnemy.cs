using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f; // Movement speed
    public float moveDistance = 5f; // Distance it moves before turning

    private Vector3 startPosition;
    private int direction = 1; // Movement direction

    [Header("Damage & Knockback Settings")]
    public float damageAmount = 10f; // Damage dealt to the player
    public float knockbackForce = 10f; // Strength of knockback
    public float upwardKnockback = 2f; // Adds slight lift
    public float knockbackDuration = 0.2f; // Duration of the knockback effect

    void Start()
    {
        startPosition = transform.position; // Store starting position
    }

    void Update()
    {
        MoveEnemy();
    }

    void MoveEnemy()
    {
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        // Reverse direction when reaching the movement limit
        if (Mathf.Abs(transform.position.x - startPosition.x) >= moveDistance)
        {
            direction *= -1; // Flip direction
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Ensure the player has "Player" tag
        {
            CharacterHealth playerHealth = collision.gameObject.GetComponent<CharacterHealth>();
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("Player hit! Damage: " + damageAmount);
            }

            if (playerRb != null)
            {
                Vector3 knockbackDirection = (collision.transform.position - transform.position).normalized;
                knockbackDirection.y = 0.2f; // Add slight upward force
                playerRb.velocity = Vector3.zero; // Reset velocity before applying force
                playerRb.AddForce(knockbackDirection * knockbackForce + Vector3.up * upwardKnockback, ForceMode.Impulse);
            }
        }
    }
}
