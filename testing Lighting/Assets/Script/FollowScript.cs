using UnityEngine;

public class FollowScript : MonoBehaviour
{
    public Transform target; 
    private Vector3 offset;  

    void Start()
    {
        
        if (target != null)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            
            transform.position = target.position + offset;

           
            transform.rotation = Quaternion.identity; 
        }
    }
}
