using UnityEngine;

public class BikeWallBlocker : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 lastSafePosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastSafePosition = transform.position;
    }

    void Update()
    {
        // Update safe position only when not moving into a wall
        if (!isBlocked)
        {
            lastSafePosition = transform.position;
        }
    }

    private bool isBlocked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Debug.Log("Wall hit! Reverting bike position.");
            isBlocked = true;

            // Stop movement
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Snap back to last safe spot
            transform.position = lastSafePosition;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            isBlocked = false;
        }
    }
}
