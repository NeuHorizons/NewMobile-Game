using UnityEngine;
using System.Collections.Generic;

public class PlatformController : MonoBehaviour
{
    private Vector3 platformLastPosition; // Tracks the platform's last position
    private Vector3 platformMovement; // Tracks the platform's movement delta
    private HashSet<Rigidbody> objectsOnPlatform = new HashSet<Rigidbody>(); // Stores objects on the platform

    private void Start()
    {
        platformLastPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check for Rigidbody component to track objects on the platform
        if (collision.rigidbody != null)
        {
            Debug.Log($"{collision.gameObject.name} is now on the platform.");
            objectsOnPlatform.Add(collision.rigidbody);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Remove objects from tracking when they leave the platform
        if (collision.rigidbody != null)
        {
            Debug.Log($"{collision.gameObject.name} left the platform.");
            objectsOnPlatform.Remove(collision.rigidbody);
        }
    }

    private void FixedUpdate()
    {
        platformMovement = transform.position - platformLastPosition;

        // Move each object on the platform
        foreach (var rb in objectsOnPlatform)
        {
            rb.MovePosition(rb.position + platformMovement); // Moves objects with the platform
        }

        platformLastPosition = transform.position;
    }
}