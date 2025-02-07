using UnityEngine;
using UnityEngine.Events;

public class GameActionTrigger : MonoBehaviour
{
    [Header("Collider Events")]
    public UnityEvent onEnterAction;
    public UnityEvent onExitAction;

    public RequiredItemSO requiredItem; // The item needed to open the door

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null && playerInventory.HasRequiredItem())
        {
            onEnterAction.Invoke(); // Open the door
            Debug.Log("Door Opened!");
        }
        else
        {
            Debug.Log("You need the required item to open this door!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        onExitAction.Invoke();
    }
}