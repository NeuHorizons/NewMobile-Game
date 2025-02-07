using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public RequiredItemSO itemData; // Reference to the required item

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            playerInventory.CollectItem(itemData);
            Destroy(gameObject); // Remove the item after pickup
        }
    }
}