using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FireflyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float changeDirectionInterval = 2f;
    public float flutterIntensity = 1f;
    public float flutterFrequency = 5f;

    [Header("Emission Settings")]
    public float emissionMin = 0.2f;
    public float emissionMax = 2f;
    public float emissionFlickerSpeed = 1f;
    public float minEmissionPauseDuration = 0.5f;

    [Header("Emission Aesthetic Enhancements")]
    [Tooltip("Duration (in seconds) for the fade-in effect after going dark.")]
    public float fadeInDuration = 3f; // Extremely gradual fade-in
    [Tooltip("Animation curve to control the smoothness of the fade-in.")]
    public AnimationCurve fadeInCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [Tooltip("Toggle to enable a color gradient effect for the glow.")]
    public bool useEmissionGradient = true;
    [Tooltip("Color gradient for the emission (from dark to bright, e.g., warm yellow).")]
    public Gradient emissionColorGradient;

    [Header("Facing")]
    public Transform faceTarget; // The object to face (e.g., the camera)

    private Vector3 targetDirection;
    private float timeToChangeDirection;
    private Material fireflyMaterial;

    private float emissionPhase = 0f;
    private bool isPausedAtMin = false;
    private float minPauseTimer = 0f;
    private bool isFadingIn = false;
    private float fadeInTimer = 0f;

    void Start()
    {
        // Look for a GameObject tagged "MainCamera" if faceTarget is not assigned.
        if (faceTarget == null)
        {
            GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
            if (mainCam != null)
            {
                faceTarget = mainCam.transform;
            }
        }

        PickNewDirection();
        fireflyMaterial = GetComponent<Renderer>().material;
        fireflyMaterial.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        HandleMovement();
        HandleEmission();
        HandleFacing();
    }

    void HandleMovement()
    {
        timeToChangeDirection -= Time.deltaTime;
        if (timeToChangeDirection <= 0f)
        {
            PickNewDirection();
        }

        Vector3 flutter = new Vector3(
            Mathf.Sin(Time.time * flutterFrequency),
            Mathf.Cos(Time.time * flutterFrequency * 1.2f),
            Mathf.Sin(Time.time * flutterFrequency * 0.8f)
        ) * flutterIntensity;

        transform.position += (targetDirection + flutter) * moveSpeed * Time.deltaTime;
    }

    void HandleEmission()
    {
        // If we're in the dark pause state, hold emission at minimum until the timer runs out.
        if (isPausedAtMin)
        {
            minPauseTimer -= Time.deltaTime;
            SetEmission(emissionMin);

            if (minPauseTimer <= 0f)
            {
                isPausedAtMin = false;
                isFadingIn = true;
                fadeInTimer = 0f;
            }
            return;
        }

        // In fade-in mode: use our smooth animation curve for a gradual return to full intensity.
        if (isFadingIn)
        {
            fadeInTimer += Time.deltaTime;
            float fadeProgress = Mathf.Clamp01(fadeInTimer / fadeInDuration);
            float curveValue = fadeInCurve.Evaluate(fadeProgress);
            float emissionStrength = Mathf.Lerp(emissionMin, emissionMax, curveValue);
            SetEmission(emissionStrength);

            if (fadeProgress >= 1f)
            {
                isFadingIn = false;
                emissionPhase = 0f; // Reset the phase for a smooth sine flicker.
            }
            return;
        }

        // Normal flickering effect: blend sine-based flicker with Perlin noise for a more organic variation.
        emissionPhase += Time.deltaTime * emissionFlickerSpeed;
        float sineValue = (Mathf.Sin(emissionPhase) + 1f) / 2f;

        // Check if the sine triggers a near-dark state.
        if (sineValue <= 0.01f)
        {
            isPausedAtMin = true;
            minPauseTimer = minEmissionPauseDuration;
            return;
        }

        // Blend 30% Perlin noise with 70% sine value.
        float noise = Mathf.PerlinNoise(Time.time * emissionFlickerSpeed, 0f);
        float blendedValue = Mathf.Lerp(sineValue, noise, 0.3f);
        float flickerEmission = Mathf.Lerp(emissionMin, emissionMax, blendedValue);
        SetEmission(flickerEmission);
    }

    void HandleFacing()
    {
        if (faceTarget != null)
        {
            Vector3 direction = faceTarget.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up) * Quaternion.Euler(90f, 0f, 0f);
        }
    }

    // Adjusts the emission color based on the intensity.
    void SetEmission(float intensity)
    {
        float normalizedIntensity = Mathf.InverseLerp(emissionMin, emissionMax, intensity);
        Color baseColor = useEmissionGradient ? emissionColorGradient.Evaluate(normalizedIntensity) : fireflyMaterial.color;
        Color finalColor = baseColor * Mathf.LinearToGammaSpace(intensity);
        fireflyMaterial.SetColor("_EmissionColor", finalColor);
    }

    void PickNewDirection()
    {
        targetDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-0.5f, 1f),
            0f // Keeping it 2D (Z = 0). Adjust if needed.
        ).normalized;

        timeToChangeDirection = changeDirectionInterval;
    }
}
