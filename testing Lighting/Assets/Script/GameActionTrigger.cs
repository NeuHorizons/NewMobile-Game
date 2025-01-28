using UnityEngine;
using UnityEngine.Events;

public class GameActionTrigger : MonoBehaviour
{
    [Header("Collider Events")]
    public UnityEvent onEnterAction; 
    public UnityEvent onExitAction; 

    private void OnTriggerEnter(Collider other)
    {
        

        onEnterAction.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        
        onExitAction.Invoke();
    }
}
