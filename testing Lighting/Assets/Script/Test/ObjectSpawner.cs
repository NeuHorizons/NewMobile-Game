using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject objectPrefab;
    public int desiredCount = 10;
    
    public bool useSphereArea = false;
    public float radius = 10f;
    public Vector2 boxSize = new Vector2(10f, 5f); // XY only

    [Tooltip("Offset from spawner in world space")]
    public Vector3 spawnOffset = new Vector3(0f, 0f, -5f);

    [Header("Spawn Timing")]
    [Tooltip("Interval (in seconds) between spawning/replacing objects")]
    public float spawnInterval = 1f;

    [Header("Spawn Aesthetic")]
    [Tooltip("Initial emission brightness when a firefly spawns (should be set to a low value)")]
    public float spawnEmissionMin = 0.2f;

    [Header("Update Timing")]
    public float checkInterval = 1f;

    private List<GameObject> activeObjects = new List<GameObject>();
    private float checkTimer = 0f;

    void Start()
    {
        // Start the continuous spawning routine.
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;
            CleanUpOutOfBounds();
        }
    }

    IEnumerator SpawnRoutine()
    {
        // Continuously spawn objects at the fixed interval until we reach desiredCount.
        while (true)
        {
            if (activeObjects.Count < desiredCount)
            {
                SpawnObject();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void CleanUpOutOfBounds()
    {
        // Remove objects that have left the spawn area.
        for (int i = activeObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = activeObjects[i];
            if (obj == null)
            {
                activeObjects.RemoveAt(i);
                continue;
            }
            
            if (!IsWithinArea(obj.transform.position))
            {
                Destroy(obj);
                activeObjects.RemoveAt(i);
            }
        }
    }

    void SpawnObject()
    {
        // Calculate the world space center for the spawn area.
        Vector3 center = transform.position + spawnOffset;
        Vector3 spawnPos;

        if (useSphereArea)
        {
            Vector2 randomXY = Random.insideUnitCircle * radius;
            spawnPos = center + new Vector3(randomXY.x, randomXY.y, 0f);
        }
        else
        {
            float randX = Random.Range(-boxSize.x / 2f, boxSize.x / 2f);
            float randY = Random.Range(-boxSize.y / 2f, boxSize.y / 2f);
            spawnPos = center + new Vector3(randX, randY, 0f);
        }

        GameObject newObj = Instantiate(objectPrefab, spawnPos, Quaternion.identity);
        activeObjects.Add(newObj);

        // Immediately set the new object's emission to the minimum value
        Renderer rend = newObj.GetComponent<Renderer>();
        if (rend != null)
        {
            Color baseColor = rend.material.color;
            Color emissionColor = baseColor * Mathf.LinearToGammaSpace(spawnEmissionMin);
            rend.material.SetColor("_EmissionColor", emissionColor);
        }
    }

    bool IsWithinArea(Vector3 worldPos)
    {
        Vector3 center = transform.position + spawnOffset;
        Vector3 offset = worldPos - center;
        offset.z = 0f; // Ignore the z-axis

        if (useSphereArea)
        {
            return new Vector2(offset.x, offset.y).magnitude <= radius;
        }
        else
        {
            return Mathf.Abs(offset.x) <= boxSize.x / 2f &&
                   Mathf.Abs(offset.y) <= boxSize.y / 2f;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center = transform.position + spawnOffset;

        if (useSphereArea)
        {
            Gizmos.DrawWireSphere(center, radius);
        }
        else
        {
            // Draw an axis-aligned box (in world space).
            Gizmos.DrawWireCube(center, new Vector3(boxSize.x, boxSize.y, 0.01f));
        }
    }
}
