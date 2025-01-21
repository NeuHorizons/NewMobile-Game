using UnityEngine;

public class RainDroplet : MonoBehaviour
{
    public float fallSpeed = 5f; // Speed of the rain's fall
    public GameObject splashEffect; // Effect when rain hits the ground or player

    void Update()
    {
        // Move the rain droplet downward
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Trigger splash effect
        if (splashEffect != null)
        {
            Instantiate(splashEffect, transform.position, Quaternion.identity);
        }

        // Check if the collided object is the player
        if (collision.CompareTag("Player"))
        {
            // Deal damage to the player
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }

        // Destroy the rain droplet regardless of what it hits
        Destroy(gameObject);
    }
}