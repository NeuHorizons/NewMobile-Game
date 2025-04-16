using UnityEngine;

public class HealingZone : MonoBehaviour
{
    [Tooltip("Amount of health restored per second.")]
    public float healingRate = 5f;

    [Tooltip("Dimensions of the healing area box.")]
    public Vector3 healingAreaSize = new Vector3(5f, 5f, 5f);

    [Tooltip("Offset from the object's position where healing is applied.")]
    public Vector3 healingZoneOffset = Vector3.zero;

    // Cached reference to the player's CharacterHealth component.
    private CharacterHealth playerHealth;
    private bool playerInside = false;

    // Compute the center of the healing zone.
    private Vector3 HealingCenter => transform.position + healingZoneOffset;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth = other.GetComponent<CharacterHealth>();
            if (playerHealth != null)
            {
                playerInside = true;
                Debug.Log("Player entered healing zone.");
            }
            else
            {
                Debug.LogWarning("Player entered healing zone, but no CharacterHealth component was found.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            playerHealth = null;
            Debug.Log("Player exited healing zone.");
        }
    }

    private void Update()
    {
        if (playerInside && playerHealth != null)
        {
            // Check that the player's position is within our defined healing box.
            if (IsPlayerInsideHealingZone(playerHealth.transform.position))
            {
                float healAmount = healingRate * Time.deltaTime;
                playerHealth.Heal(healAmount);
                
            }
        }
    }

    // Check if a given position is within the box-shaped healing area.
    private bool IsPlayerInsideHealingZone(Vector3 playerPos)
    {
        Vector3 halfSize = healingAreaSize * 0.5f;
        Vector3 center = HealingCenter;

        return (Mathf.Abs(playerPos.x - center.x) <= halfSize.x &&
                Mathf.Abs(playerPos.y - center.y) <= halfSize.y &&
                Mathf.Abs(playerPos.z - center.z) <= halfSize.z);
    }

    // Visualize the healing area in the Scene view.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(HealingCenter, healingAreaSize);
    }
}
