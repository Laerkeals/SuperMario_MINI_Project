using UnityEngine;

public class PlayerPlatform : MonoBehaviour
{
    private Rigidbody rb;
    private Transform currentPlatform;
    private Vector3 lastPlatformPosition;
    private Vector3 playerOffset;
    private PlatformManager.Platform platformScript; // Reference to the platform script

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player lands on a platform
        if (other.transform.parent != null && other.transform.parent.CompareTag("Platform"))
        {
            currentPlatform = other.transform.parent;
            platformScript = currentPlatform.GetComponent<PlatformManager.Platform>();

            if (platformScript != null)
            {
                lastPlatformPosition = platformScript.previousPosition;
                playerOffset = transform.position - currentPlatform.position;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (currentPlatform != null && platformScript != null)
        {
            // Apply the platform's velocity to the player
            Vector3 platformVelocity = platformScript.velocity;
            rb.MovePosition(rb.position + platformVelocity * Time.fixedDeltaTime);

            // Update the last known platform position for smoother transitions
            lastPlatformPosition = platformScript.previousPosition;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.CompareTag("Platform"))
        {
            // Reset platform reference when leaving the platform
            currentPlatform = null;
            platformScript = null;
        }
    }
}
