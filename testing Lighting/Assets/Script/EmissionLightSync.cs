using UnityEngine;

public class LightEmissionController : MonoBehaviour
{
    [Header("Reference Light")]
    public Light referenceLight; // The light source to control

    [Header("Emission Settings")]
    public bool enableEmissionPulse = false; // Toggle emission pulsation
    public float emissionPulseSpeed = 1f; // Speed of emission pulsing
    public float minEmissionIntensity = 0.1f; // Minimum emission intensity
    public float maxEmissionIntensity = 2f; // Maximum emission intensity

    [Header("Light Settings")]
    public bool enableLightPulse = false; // Toggle light pulsation
    public float lightPulseSpeed = 1f; // Speed of light pulsing
    public float minLightIntensity = 0.5f; // Minimum light intensity
    public float maxLightIntensity = 3f; // Maximum light intensity

    private Renderer objRenderer;
    private Material objMaterial;
    private Color baseEmissionColor;
    private float emissionTimer = 0f;
    private float lightTimer = 0f;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();

        if (objRenderer != null)
        {
            objMaterial = objRenderer.material;
            baseEmissionColor = objMaterial.GetColor("_EmissionColor");
            objMaterial.EnableKeyword("_EMISSION");
        }

        if (referenceLight == null)
        {
            Debug.LogWarning("Reference Light is not assigned!");
        }
    }

    void Update()
    {
        if (objMaterial != null)
        {
            UpdateEmission();
        }

        if (referenceLight != null)
        {
            UpdateLight();
        }
    }

    void UpdateEmission()
    {
        float emissionIntensity = maxEmissionIntensity; // Default to max
        if (enableEmissionPulse)
        {
            emissionTimer += Time.deltaTime * emissionPulseSpeed;
            emissionIntensity = Mathf.Lerp(minEmissionIntensity, maxEmissionIntensity, Mathf.PingPong(emissionTimer, 1f));
        }

        Color emissionColor = baseEmissionColor * emissionIntensity;
        objMaterial.SetColor("_EmissionColor", emissionColor);
        DynamicGI.SetEmissive(objRenderer, emissionColor);
    }

    void UpdateLight()
    {
        float lightIntensity = maxLightIntensity; // Default to max
        if (enableLightPulse)
        {
            lightTimer += Time.deltaTime * lightPulseSpeed;
            lightIntensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, Mathf.PingPong(lightTimer, 1f));
        }

        referenceLight.intensity = lightIntensity;
    }
}
