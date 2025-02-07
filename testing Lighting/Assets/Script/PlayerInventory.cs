using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public RequiredItemSO requiredItem; // The item the player can collect
    private bool hasItem = false; // Tracks if the player has the item

    public void CollectItem(RequiredItemSO item)
    {
        if (item == requiredItem)
        {
            hasItem = true;
            Debug.Log("Collected: " + item.itemName);
        }
    }

    public bool HasRequiredItem()
    {
        return hasItem;
    }
}