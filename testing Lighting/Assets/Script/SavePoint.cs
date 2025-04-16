using UnityEngine;

public class SavePoint : MonoBehaviour
{
    // Reference to the PlayerData ScriptableObject asset
    public PlayerData playerData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Update the saved position to the player's current position
            playerData.savedPosition = other.transform.position;
            
            // Save the player data using the SaveSystem
            SaveSystem.SavePlayerData(playerData);
            Debug.Log("Game saved at position: " + playerData.savedPosition);
        }
    }
}
