using UnityEngine;
using System.Collections.Generic;

public class PlatformController : MonoBehaviour
{
    private Vector3 platformLastPosition; 
    private Vector3 platformMovement; 
    private HashSet<Rigidbody> objectsOnPlatform = new HashSet<Rigidbody>(); 

    private void Start()
    {
        platformLastPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.rigidbody != null)
        {
            
            objectsOnPlatform.Add(collision.rigidbody);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
        if (collision.rigidbody != null)
        {
           
            objectsOnPlatform.Remove(collision.rigidbody);
        }
    }

    private void FixedUpdate()
    {
        platformMovement = transform.position - platformLastPosition;

        
        foreach (var rb in objectsOnPlatform)
        {
            rb.MovePosition(rb.position + platformMovement); 
        }

        platformLastPosition = transform.position;
    }
}