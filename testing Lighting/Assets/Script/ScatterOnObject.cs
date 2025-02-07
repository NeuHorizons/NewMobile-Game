using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ScatterOnObject : MonoBehaviour
{
    [Header("Scatter Settings")]
    public GameObject objectToScatter;  // Prefab to scatter
    public GameObject targetObject;    // Target object to scatter on
    public int numberOfObjects = 10;   // Number of objects to scatter
    public bool alignToSurface = true; // Align scattered objects to the surface normal

    [Header("Random Size Settings")]
    public bool randomizeSize = false;

    [Tooltip("Minimum and maximum size for the X axis")]
    public Vector2 sizeRangeX = new Vector2(0.5f, 2f);

    [Tooltip("Minimum and maximum size for the Y axis")]
    public Vector2 sizeRangeY = new Vector2(0.5f, 2f);

    [Tooltip("Minimum and maximum size for the Z axis")]
    public Vector2 sizeRangeZ = new Vector2(0.5f, 2f);

    public void Scatter()
    {
        if (objectToScatter == null || targetObject == null)
        {
            Debug.LogError("Both the object to scatter and the target object must be assigned!");
            return;
        }

        // Clear existing scattered objects
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        // Get the target object's bounds
        Collider targetCollider = targetObject.GetComponent<Collider>();
        if (targetCollider == null)
        {
            Debug.LogError("The target object must have a Collider!");
            return;
        }

        for (int i = 0; i < numberOfObjects; i++)
        {
            // Generate a random position within the bounds of the target object
            Vector3 randomPosition = new Vector3(
                Random.Range(targetCollider.bounds.min.x, targetCollider.bounds.max.x),
                Random.Range(targetCollider.bounds.min.y, targetCollider.bounds.max.y),
                Random.Range(targetCollider.bounds.min.z, targetCollider.bounds.max.z)
            );

            // Check if the random position is on the surface of the target object
            if (Physics.Raycast(randomPosition + Vector3.up * 10, Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == targetObject)
                {
                    // Instantiate the object at the hit point
                    GameObject instance = Instantiate(objectToScatter, hit.point, Quaternion.identity, transform);

                    // Align to surface normal if enabled
                    if (alignToSurface)
                    {
                        instance.transform.up = hit.normal;
                    }

                    // Randomize size if enabled
                    if (randomizeSize)
                    {
                        float randomScaleX = Random.Range(sizeRangeX.x, sizeRangeX.y);
                        float randomScaleY = Random.Range(sizeRangeY.x, sizeRangeY.y);
                        float randomScaleZ = Random.Range(sizeRangeZ.x, sizeRangeZ.y);
                        instance.transform.localScale = new Vector3(randomScaleX, randomScaleY, randomScaleZ);
                    }

                    instance.name = $"{objectToScatter.name}_{i}";
                }
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ScatterOnObject))]
public class ScatterOnObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ScatterOnObject scatterScript = (ScatterOnObject)target;

        if (GUILayout.Button("Scatter Objects"))
        {
            scatterScript.Scatter();
        }
    }
}
#endif
