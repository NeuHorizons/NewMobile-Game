using UnityEngine;

[CreateAssetMenu(fileName = "NewHealthData", menuName = "ScriptableObjects/HealthData", order = 1)]
public class HealthData : ScriptableObject
{
    [Header("Health Properties")]
    public float maxHealth = 100f; 
    public float currentHealth; 

    
    public void InitializeHealth()
    {
        currentHealth = maxHealth;
    }

   
    public void ModifyHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

   
    public float GetNormalizedHealth()
    {
        return currentHealth / maxHealth;
    }

   
    public bool IsAlive()
    {
        return currentHealth > 0;
    }
}