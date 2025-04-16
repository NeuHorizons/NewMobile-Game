using UnityEngine;

public class DamageChecker : MonoBehaviour
{
    public CharacterHealth characterHealth;
    public float damageAmount = 10f;

    void OnTriggerEnter(Collider other)
    {
        // Only trigger damage if the collider belongs to the player
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player collision detected - applying damage.");
            characterHealth.TakeDamage(damageAmount);
        }
    }
}