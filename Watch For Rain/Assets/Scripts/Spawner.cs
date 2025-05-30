using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn; 
    public float minX = -5f; 
    public float maxX = 5f;  
    public float yPosition = 0f; 
    public float spawnInterval = 2f; 

    private void Start()
    {
        
        InvokeRepeating(nameof(SpawnPrefab), spawnInterval, spawnInterval);
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
    }
}
