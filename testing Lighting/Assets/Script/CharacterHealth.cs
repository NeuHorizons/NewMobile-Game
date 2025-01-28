using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    public HealthData healthData; 
    public Light characterLight;
    public Slider healthSlider; 

    [Header("Light Settings")]
    public float maxLightIntensity = 2f; 
    public float minLightIntensity = 0f; 

    private void Start()
    {
        if (healthData != null)
        {
            healthData.InitializeHealth();
        }

        if (healthSlider != null)
        {
            healthSlider.maxValue = healthData.maxHealth;
            healthSlider.value = healthData.currentHealth;
        }

        UpdateLightIntensity();
    }

    public void TakeDamage(float damage)
    {
        if (healthData != null)
        {
            healthData.ModifyHealth(-damage);

            if (healthSlider != null)
            {
                healthSlider.value = healthData.currentHealth;
            }

            UpdateLightIntensity();

            if (!healthData.IsAlive())
            {
                Die();
            }
        }
    }

    private void UpdateLightIntensity()
    {
        if (characterLight != null)
        {
            float normalizedHealth = healthData.GetNormalizedHealth();
            characterLight.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, normalizedHealth);
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
       
    }
}