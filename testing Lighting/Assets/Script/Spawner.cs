using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float minX = -5f;
    public float maxX = 5f;
    public float yPosition = 0f;
    public float spawnInterval = 2f;
    public float activationRange = 10f;

    private Transform player;
    private bool isSpawning;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating(nameof(CheckAndSpawn), 0f, spawnInterval);
    }

    private void CheckAndSpawn()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= activationRange)
        {
            SpawnPrefab();
        }
    }

    private void SpawnPrefab()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(randomX, yPosition, 0f);
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(minX, yPosition, 0f), new Vector3(maxX, yPosition, 0f));

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, activationRange);
    }
}