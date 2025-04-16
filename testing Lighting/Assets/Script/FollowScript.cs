using UnityEngine;

public class FollowScript : MonoBehaviour
{
    // Set this offset in the Inspector (or leave the default)
    public Vector3 offset = new Vector3(0, 5, -10);
    public Transform target; 

    void Start()
    {
        if (target != null)
        {
            // Immediately snap the camera to the target's position plus the offset.
            transform.position = target.position + offset;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Continuously update the camera's position to follow the target.
            transform.position = target.position + offset;
            transform.rotation = Quaternion.identity;
        }
    }
}