using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode] // Allows the script to run in Edit mode
public class ScatterObjects : MonoBehaviour
{
    [Header("Scatter Settings")]
    public GameObject objectToScatter; // Prefab to scatter
    public int numberOfObjects = 10;   // Number of objects to scatter
    public float radius = 5f;          // Radius of the scatter area

    [Header("Random Size Settings")]
    public bool randomizeSize = false;

    [Tooltip("Minimum and maximum size for the X axis")]
    public Vector2 sizeRangeX = new Vector2(0.5f, 2f);

    [Tooltip("Minimum and maximum size for the Y axis")]
    public Vector2 sizeRangeY = new Vector2(0.5f, 2f);

    [Tooltip("Minimum and maximum size for the Z axis")]
    public Vector2 sizeRangeZ = new Vector2(0.5f, 2f);

    [Header("Manual Placement Tool")]
    public GameObject placementCube;   // Cube to visualize placement
    public bool showPlacementCubes = true;

    private void OnDrawGizmos()
    {
        if (!showPlacementCubes || placementCube == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(placementCube.transform.position, Vector3.one);
    }

    public void Scatter()
    {
        if (objectToScatter == null)
        {
            Debug.LogError("No object assigned to scatter!");
            return;
        }

        // Clear existing children (optional)
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        // Scatter objects
        for (int i = 0; i < numberOfObjects; i++)
        {
            // Generate a random position within a circle
            Vector2 randomCircle = Random.insideUnitCircle * radius;
            Vector3 randomPosition = new Vector3(randomCircle.x, 0, randomCircle.y) + transform.position;

            // Instantiate the object as a child of this GameObject
            GameObject instance = Instantiate(objectToScatter, randomPosition, Quaternion.identity, transform);
            instance.name = $"{objectToScatter.name}_{i}";

            // Randomize the size if enabled
            if (randomizeSize)
            {
                float randomScaleX = Random.Range(sizeRangeX.x, sizeRangeX.y);
                float randomScaleY = Random.Range(sizeRangeY.x, sizeRangeY.y);
                float randomScaleZ = Random.Range(sizeRangeZ.x, sizeRangeZ.y);
                instance.transform.localScale = new Vector3(randomScaleX, randomScaleY, randomScaleZ);
            }
        }
    }

    public void PlaceCube(Vector3 position)
    {
        if (placementCube == null)
        {
            Debug.LogError("No placement cube assigned!");
            return;
        }

        Instantiate(placementCube, position, Quaternion.identity, transform);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ScatterObjects))]
public class ScatterObjectsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ScatterObjects scatterScript = (ScatterObjects)target;

        if (GUILayout.Button("Scatter Objects"))
        {
            scatterScript.Scatter();
        }

        if (GUILayout.Button("Place Cube"))
        {
            Ray ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay(Event.current.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                scatterScript.PlaceCube(hit.point);
            }
        }
    }
}
#endif
