using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    public static event Action OnPlayerDeath;

    public HealthData healthData;
    public Light characterLight;
    public Slider healthSlider;
    public Renderer characterRenderer;

    [Header("Light Settings")]
    public float maxLightIntensity = 2f;
    public float minLightIntensity = 0f;

    [Header("Emission Settings")]
    public float maxEmissionIntensity = 2f;
    public float minEmissionIntensity = 0f;

    private Material characterMaterial;
    private Color baseEmissionColor;

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

        if (characterRenderer != null)
        {
            characterMaterial = characterRenderer.material;
            baseEmissionColor = characterMaterial.GetColor("_EmissionColor");
            characterMaterial.EnableKeyword("_EMISSION");
        }

        UpdateLightAndEmission();
    }

    public void TakeDamage(float damage)
    {
        if (healthData != null)
        {
            healthData.ModifyHealth(-damage);
            healthData.currentHealth = Mathf.Max(healthData.currentHealth, 0);

            if (healthSlider != null)
            {
                healthSlider.value = healthData.currentHealth;
            }

            UpdateLightAndEmission();

            if (!healthData.IsAlive())
            {
                Die();
            }
        }
    }

    public void Heal(float healAmount)
    {
        if (healthData != null)
        {
            float previousHealth = healthData.currentHealth;
            healthData.currentHealth = Mathf.Clamp(healthData.currentHealth + healAmount, 0, healthData.maxHealth);

            if (healthSlider != null)
            {
                healthSlider.value = healthData.currentHealth;
            }

            

            UpdateLightAndEmission();
        }
    }

    private void UpdateLightAndEmission()
    {
        float normalizedHealth = healthData.GetNormalizedHealth();

        if (characterLight != null)
        {
            characterLight.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, normalizedHealth);
        }

        if (characterMaterial != null)
        {
            float emissionIntensity = Mathf.Lerp(minEmissionIntensity, maxEmissionIntensity, normalizedHealth);
            Color emissionColor = baseEmissionColor * emissionIntensity;
            characterMaterial.SetColor("_EmissionColor", emissionColor);
            DynamicGI.SetEmissive(characterRenderer, emissionColor);
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        OnPlayerDeath?.Invoke();
    }
}
