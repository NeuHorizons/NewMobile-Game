using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the player
    private int currentHealth; // Current health of the player

    public Slider healthBar; // Reference to a UI slider for the health bar

    public GameObject deathEffect; // Effect to show when the player dies

    void Start()
    {
        // Set the player's health to the maximum at the start
        currentHealth = maxHealth;

        // Update the health bar UI
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        // Reduce the player's health
        currentHealth -= damage;

        // Clamp the health to prevent it from going negative
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update the health bar UI
        UpdateHealthBar();

        // Check if the player is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        // Increase the player's health
        currentHealth += amount;

        // Clamp the health to prevent it from exceeding the max
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update the health bar UI
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            // Set the slider value to the normalized health percentage
            healthBar.value = (float)currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        // Play death effect if assigned
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Destroy the player object or handle game-over logic
        Destroy(gameObject);

        // Optionally trigger a game-over screen or respawn logic
        Debug.Log("Player has died!");
    }
}