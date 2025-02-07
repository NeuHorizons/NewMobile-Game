using System.Collections;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public enum MoveAxis { X, Y, Z } // Enum to select axis
    public MoveAxis selectedAxis = MoveAxis.Z; // Default to Z-axis

    public float targetDistance = 5f; // How far the platform moves
    public float waitTime = 1f; // Time to wait before reversing
    public float moveDuration = 2f; // Time it takes to move between positions
    public GameObject platform;

    private Vector3 startPosition; // Starting position of the platform
    private Vector3 targetPosition; // Target position of the platform

    void Start()
    {
        if (platform != null)
        {
            startPosition = platform.transform.position;
            targetPosition = CalculateTargetPosition(); // Set the target position
            StartCoroutine(MovePlatform());
        }
    }

    // Determines the target position based on the selected axis
    Vector3 CalculateTargetPosition()
    {
        switch (selectedAxis)
        {
            case MoveAxis.X:
                return startPosition + Vector3.right * targetDistance; // Move along X
            case MoveAxis.Y:
                return startPosition + Vector3.up * targetDistance; // Move along Y
            case MoveAxis.Z:
                return startPosition + Vector3.forward * targetDistance; // Move along Z
            default:
                return startPosition;
        }
    }

    IEnumerator MovePlatform()
    {
        while (true) // Loop the movement
        {
            // Move to the target position
            yield return MoveToPosition(targetPosition);

            // Wait at the target position
            yield return new WaitForSeconds(waitTime);

            // Move back to the starting position
            yield return MoveToPosition(startPosition);

            // Wait at the starting position
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator MoveToPosition(Vector3 destination)
    {
        float elapsedTime = 0f; // Track elapsed time

        // Get the platform's current position
        Vector3 initialPosition = platform.transform.position;

        while (elapsedTime < moveDuration)
        {
            // Interpolate position based on elapsed time
            platform.transform.position = Vector3.Lerp(initialPosition, destination, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime; // Increment elapsed time
            yield return null; // Wait until the next frame
        }

        // Ensure the platform reaches the destination exactly
        platform.transform.position = destination;
    }
}
