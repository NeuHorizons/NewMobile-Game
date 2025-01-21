using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn; // The prefab to spawn
    public float minX = -5f; // Minimum x position for spawning
    public float maxX = 5f;  // Maximum x position for spawning
    public float yPosition = 0f; // Fixed y position for all spawned prefabs
    public float spawnInterval = 2f; // Time interval between spawns

    private void Start()
    {
        // Start the spawning process
        InvokeRepeating(nameof(SpawnPrefab), spawnInterval, spawnInterval);
    }

    private void SpawnPrefab()
    {
        // Randomize the x position
        float randomX = Random.Range(minX, maxX);

        // Create the spawn position
        Vector3 spawnPosition = new Vector3(randomX, yPosition, 0f);

        // Instantiate the prefab at the spawn position
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        // Visualize the spawn range in the Scene view
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(minX, yPosition, 0f), new Vector3(maxX, yPosition, 0f));
    }
}
