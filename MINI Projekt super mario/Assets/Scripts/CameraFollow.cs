using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player; // Reference to the player transform
    public Vector3 offset = new Vector3(0, 2, -5); // Offset behind the player
    public float rotationSpeed = 5f; // Speed at which the player rotates with the mouse

    private float mouseX; // Mouse movement on the X-axis

    void LateUpdate()
    {
        // Get horizontal mouse movement
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;

        // Rotate the player based on mouse movement
        player.rotation = Quaternion.Euler(0, mouseX, 0);

        // Position the camera behind the player
        Vector3 desiredPosition = player.position + player.rotation * offset;
        transform.position = desiredPosition;

        // Look at the player
        transform.LookAt(player.position + Vector3.up * 1.5f); // Adjust height to look slightly above the player
    }
}
