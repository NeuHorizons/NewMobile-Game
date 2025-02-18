using UnityEngine;

public class DamageChecker : MonoBehaviour
{
    public CharacterHealth characterHealth;
    public float damageAmount = 10f;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("lol");
        characterHealth.TakeDamage(damageAmount);
    }
}
